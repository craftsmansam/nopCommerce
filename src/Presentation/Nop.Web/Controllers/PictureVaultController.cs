using System;
using System.IO;
using System.Linq;
using Craftsman.Data;
using Craftsman.DesignByContract;
using Craftsman.IO;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Media;
using Nop.Services.Configuration;
using Nop.Services.PictureVault;
using Nop.Services.Security;
using Nop.Web.Models.PictureVault;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PictureVaultController : BasePublicController
    {
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IPictureVaultService _pictureVaultService;
        private readonly MediaSettings _mediaSettings;
        private readonly ISettingService _settingService;

        public PictureVaultController(IPermissionService permissionService, IWorkContext workContext, IPictureVaultService pictureVaultService, MediaSettings mediaSettings, ISettingService settingService)
        {
            _permissionService = permissionService;
            _workContext = workContext;
            _pictureVaultService = pictureVaultService;
            _mediaSettings = mediaSettings;
            _settingService = settingService;
        }

        public IActionResult BrowsePictureVault()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.PictureVault))
                return Challenge();

            var model = new BrowsePictureVaultModel();
            var itemsTable = _pictureVaultService.CustomerListPurchaseOrders(_workContext.CurrentCustomer.SalesContactID);
            model.PurchaseOrderList = itemsTable;

            return View(model);
        }

        public IActionResult PictureVault(string po)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.PictureVault))
                return Challenge();

            var itemsTable = _pictureVaultService.CustomerListPictureVaultItems(_workContext.CurrentCustomer.SalesContactID, po);

            var showPictureUrl = $"/secure/show-picture?cscid={_workContext.CurrentCustomer.SalesContactID}&id=";
            var groupedItems = itemsTable.GroupBy(x => new {x.ProcessName, x.ShopOrderNumber, x.ProjectName}, x => new { ImgSrc = $"{showPictureUrl}{x.ShopOrderPictureID}", Title = $"Taken {x.DateTimeUploaded.ToString(FormatStrings.DateTimeFormat)}"});
            var pvGroups = groupedItems.Select(x => new PictureVaultGroup($"{x.Key.ProjectName} (SO#{x.Key.ShopOrderNumber}) - {x.Key.ProcessName}", 
                                                                        x.Select(y => new PictureVaultImage(y.Title, y.ImgSrc)).ToList())).ToList();
            var model = new PictureVaultModel(po, pvGroups);

            return View(model);
        }

        public IActionResult ShowPicture(string cscid, string id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.PictureVault))
                return Challenge();

            Check.RequireNotNull(_workContext.CurrentCustomer.SalesContactID);

            if (id != null && int.TryParse(id, out var imageId))
            {
                if (cscid != null && int.TryParse(cscid, out var customerSalesContactId))
                {
                    var isPictureForMyCompany = _pictureVaultService.IsPictureForMyCompany(customerSalesContactId, imageId);

                    Check.Require(isPictureForMyCompany, "Someone is potentially trying to access some else's photos!");
                    Check.Require(_workContext.CurrentCustomer.SalesContactID == customerSalesContactId, "The logged in user is potentially trying to look at someone else's photos!");

                    var fullFilePath = _pictureVaultService.PictureVaultFileNameByShopOrderPictureId(imageId);
                    var filenameOnly = Path.GetFileNameWithoutExtension(fullFilePath);
                    var fileExtension = Path.GetExtension(fullFilePath).ToLower().Replace(".", "");

                    var shopOrderNumber = _pictureVaultService.PictureVaultSONumberBySOPictureId(imageId);
                    var filenameOnNetwork = Path.Combine(PictureVaultFolderAsUnc(false, shopOrderNumber), $"{filenameOnly}.{fileExtension}");
                    Check.Require(System.IO.File.Exists(filenameOnNetwork), $"The file {filenameOnNetwork} doesn't exist!");

                    var mimeTypeString = MimeType.All.First(x => x.FileExtensions.Any(y => y.Extension.ToLower() == fileExtension)).MimeTypeString;

                    using var image = Image.Load(filenameOnNetwork, out _);
                    if (image.Width > _mediaSettings.MaximumImageSize || image.Height > _mediaSettings.MaximumImageSize)
                    {
                        var resizeOptions = new ResizeOptions
                        {
                            Size = new Size(_mediaSettings.MaximumImageSize, _mediaSettings.MaximumImageSize),
                            Sampler = KnownResamplers.Lanczos3,
                            Compand = true,
                            Mode = ResizeMode.Max
                        };
                        image.Mutate(x => x.Resize(resizeOptions).AutoOrient());

                        var encoder = new JpegEncoder
                        {
                            Quality = _mediaSettings.DefaultImageQuality
                        };

                        using var memoryStream = new MemoryStream();
                        image.Save(memoryStream, encoder);
                        memoryStream.Position = 0;

                       return File(memoryStream.ToArray(), mimeTypeString);
                    }

                    return File(filenameOnNetwork, mimeTypeString);

                }
            }

            throw new ArgumentException($"{nameof(ShowPicture)} was called with invalid arguments!");
        }

        public string PictureVaultFolderAsUnc(bool internalPhoto, int shopOrderNumber)
        {
            var pictureVaultRootFolder = _settingService.PictureVaultRootFolder().UncPath;

            return CalculatePath(pictureVaultRootFolder, internalPhoto, shopOrderNumber);
        }

        private static string CalculatePath(string rootFolder, bool internalPhoto, int shopOrderNumber)
        {
            var rootPath = $@"{rootFolder}\SO{shopOrderNumber}";
            if (internalPhoto)
            {
                rootPath = Path.Combine(rootPath, "Internal");
            }

            return rootPath;
        }
    }
}
