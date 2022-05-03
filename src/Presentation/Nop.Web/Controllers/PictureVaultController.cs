using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> BrowsePictureVault()
        {
            if (!(await _permissionService.AuthorizeAsync(StandardPermissionProvider.PictureVault)))
                return Challenge();

            var customer = await _workContext.GetCurrentCustomerAsync();

            var model = new BrowsePictureVaultModel();
            var itemsTable = await _pictureVaultService.CustomerListPurchaseOrdersAsync(customer.SalesContactID);
            model.PurchaseOrderList = itemsTable;

            return View(model);
        }

        public async Task<IActionResult> PictureVault(string po)
        {
            if (!(await _permissionService.AuthorizeAsync(StandardPermissionProvider.PictureVault)))
                return Challenge();

            var customer = await _workContext.GetCurrentCustomerAsync();
            var itemsTable = await _pictureVaultService.CustomerListPictureVaultItemsAsync(customer.SalesContactID, po);

            var showPictureUrl = $"/secure/show-picture?cscid={customer.SalesContactID}&id=";
            var groupedItems = itemsTable.GroupBy(x => new {x.ProcessName, x.ShopOrderNumber, x.ProjectName}, x => new { ImgSrc = $"{showPictureUrl}{x.ShopOrderPictureID}", Title = $"Taken {x.DateTimeUploaded.ToString(FormatStrings.DateTimeFormat)}"});
            var pvGroups = groupedItems.Select(x => new PictureVaultGroup($"{x.Key.ProjectName} (SO#{x.Key.ShopOrderNumber}) - {x.Key.ProcessName}", 
                                                                        x.Select(y => new PictureVaultImage(y.Title, y.ImgSrc)).ToList())).ToList();
            var model = new PictureVaultModel(po, pvGroups);

            return View(model);
        }

        public async Task<IActionResult> ShowPicture(string cscid, string id)
        {
            if (!(await _permissionService.AuthorizeAsync(StandardPermissionProvider.PictureVault)))
                return Challenge();

            var customer = await _workContext.GetCurrentCustomerAsync();

            Check.RequireNotNull(customer.SalesContactID);

            if (id != null && int.TryParse(id, out var imageId))
            {
                if (cscid != null && int.TryParse(cscid, out var customerSalesContactId))
                {
                    var isPictureForMyCompany = await _pictureVaultService.IsPictureForMyCompanyAsync(customerSalesContactId, imageId);

                    Check.Require(isPictureForMyCompany, "Someone is potentially trying to access some else's photos!");
                    Check.Require(customer.SalesContactID == customerSalesContactId, "The logged in user is potentially trying to look at someone else's photos!");

                    var fullFilePath = await _pictureVaultService.PictureVaultFileNameByShopOrderPictureIdAsync(imageId);
                    var filenameOnly = Path.GetFileNameWithoutExtension(fullFilePath);
                    var fileExtension = Path.GetExtension(fullFilePath).ToLower().Replace(".", "");

                    var shopOrderNumber = await _pictureVaultService.PictureVaultSONumberBySOPictureIdAsync(imageId);
                    var filenameOnNetwork = Path.Combine(await PictureVaultFolderAsUncAsync(false, shopOrderNumber), $"{filenameOnly}.{fileExtension}");
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

        public async Task<string> PictureVaultFolderAsUncAsync(bool internalPhoto, int shopOrderNumber)
        {
            var pictureVaultRootFolder = (await _settingService.PictureVaultRootFolderAsync()).UncPath;

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
