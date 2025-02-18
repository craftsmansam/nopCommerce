using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Craftsman.Data;
using Craftsman.DesignByContract;
using Craftsman.IO;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Services.Configuration;
using Nop.Services.PictureVault;
using Nop.Services.Security;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.PictureVault;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
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

        [CheckPermission(StandardPermission.PublicStore.PICTURE_VAULT)]
        public async Task<IActionResult> BrowsePictureVaultAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var model = new BrowsePictureVaultModel();
            var itemsTable = await _pictureVaultService.CustomerListPurchaseOrdersAsync(customer.SalesContactID);
            model.PurchaseOrderList = itemsTable;

            return View(model);
        }

        [CheckPermission(StandardPermission.PublicStore.PICTURE_VAULT)]
        public async Task<IActionResult> PictureVaultAsync(string po)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var itemsTable = await _pictureVaultService.CustomerListPictureVaultItemsAsync(customer.SalesContactID, po);

            var showPictureUrl = $"/secure/show-picture?cscid={customer.SalesContactID}&id=";
            var showVideoUrl = $"/secure/show-video?cscid={customer.SalesContactID}&id=";
            var groupedItems = itemsTable.GroupBy(x => new {x.ProcessName, x.ShopOrderNumber, x.ProjectName}, x =>
            {
                var fileExtension = Path.GetExtension(x.Filename).Replace(".", string.Empty);
                var isVideo = MimeType.VideoMp4.FileExtensions.Any(ext => ext.Extension.ToLower() == fileExtension.ToLower());
                var size = (Size?)null;

                if (isVideo)
                {
                    size = GetVideoSizeAsync(x.Filename).Result;
                }

                return new
                {
                    ImgSrc = $"{showPictureUrl}{x.ShopOrderPictureID}", 
                    VideoUrl = isVideo ? $"{showVideoUrl}{x.ShopOrderPictureID}" : null, 
                    Title = $"Taken {x.DateTimeUploaded.ToString(FormatStrings.DateTimeFormat)}",
                    VideoHeight = size?.Height,
                    VideoWidth = size?.Width
                };
            });
            
            var pvGroups = groupedItems.Select(x => new PictureVaultGroup($"{x.Key.ProjectName} (SO#{x.Key.ShopOrderNumber}) - {x.Key.ProcessName}", 
                                                                        x.Select(y => new PictureVaultImage(y.Title, y.ImgSrc, y.VideoUrl, y.VideoWidth, y.VideoHeight)).ToList())).ToList();
            var model = new PictureVaultModel(po, pvGroups);

            return View(model);
        }

        private static async Task<Size> GetVideoSizeAsync(string filenameOnNetwork)
        {
            var jpgFilename = Path.ChangeExtension(filenameOnNetwork, MimeType.ImageJpg.FileExtensions[1].Extension);
            using var image = await Image.LoadAsync(jpgFilename);
            
            return new Size(image.Width, image.Height);
        }

        public async Task<IActionResult> VideoPlayerAsync(string cscid, string id)
        {
            return await ShowItemAsync(cscid, id, async (_, filenameOnNetwork) =>
            {
                var size = await GetVideoSizeAsync(filenameOnNetwork);

                var videoUrl = $"/secure/show-video?cscid={cscid}&id={id}";
                var model = new VideoPlayerModel(videoUrl, size.Width, size.Height);

                return View(model);
            });
        }

        public async Task<IActionResult> ShowVideoAsync(string cscid, string id)
        {
            return await ShowItemAsync(cscid, id, ShowVideoImplAsync);
        }

        public async Task<IActionResult> ShowPictureAsync(string cscid, string id)
        {
            return await ShowItemAsync(cscid, id, ShowPictureImplAsync);
        }

        [CheckPermission(StandardPermission.PublicStore.ALBINA_INVOICE)]
        private async Task<IActionResult> ShowItemAsync(string cscid, string id, Func<MimeType, string, Task<IActionResult>> showImpl)
        {
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

                    var mimeType = MimeType.All.First(x => x.FileExtensions.Any(y => y.Extension.ToLower() == fileExtension));

                    return await showImpl(mimeType, filenameOnNetwork);
                }
            }

            throw new ArgumentException($"{nameof(ShowPictureAsync)} was called with invalid arguments!");
        }

#pragma warning disable CS1998
        private async Task<IActionResult> ShowVideoImplAsync(MimeType mimeType, string filenameOnNetwork)
#pragma warning restore CS1998
        {
            var fileStream = new FileStream(filenameOnNetwork, FileMode.Open);
            var filename = Path.GetFileName(filenameOnNetwork);
            
            Response.Headers.Add("Content-Disposition", $"inline; filename={filename}");

            return File(fileStream, mimeType.MimeTypeString);
        }

        private async Task<IActionResult> ShowPictureImplAsync(MimeType mimeType, string filenameOnNetwork)
        {
            if (mimeType == MimeType.VideoMp4)
            {
                return await ServeVideoTypeAsync(filenameOnNetwork);
            }

            return await ServeImageTypeAsync(filenameOnNetwork, mimeType.MimeTypeString);
        }

        private async Task<IActionResult> ServeVideoTypeAsync(string filenameOnNetwork)
        {
            var mimeType = MimeType.ImageJpg;
            var jpgFilename = Path.ChangeExtension(filenameOnNetwork, mimeType.FileExtensions[1].Extension);

            return await ServeImageTypeAsync(jpgFilename, mimeType, true);
        }

        private async Task<IActionResult> ServeImageTypeAsync(string filenameOnNetwork, string mimeTypeString, bool overlayVideoIcon = false)
        {
            const string videoOverlayFilename = @"wwwroot\video_play.png";

            var image = await Image.LoadAsync(filenameOnNetwork);
            Image finalImage = null;
            Image overlayImage = null;

            try
            {
                if (image.Width > _mediaSettings.MaximumImageSize || image.Height > _mediaSettings.MaximumImageSize)
                {
                    var resizeOptions = new ResizeOptions { Size = new Size(_mediaSettings.MaximumImageSize, _mediaSettings.MaximumImageSize), Sampler = KnownResamplers.Lanczos3, Compand = true, Mode = ResizeMode.Max };
                    image.Mutate(x => x.Resize(resizeOptions).AutoOrient());
                }

                if (overlayVideoIcon)
                {
                    overlayImage = await Image.LoadAsync(videoOverlayFilename);
                    if (overlayImage.Width > image.Width || overlayImage.Height > image.Height)
                    {
                        var resize = new ResizeOptions { Size = new Size(image.Width, image.Height) };
                        overlayImage.Mutate(x => x.Resize(resize).AutoOrient());
                    }

                    finalImage = new Image<Rgba32>(image.Width, image.Height);

                    finalImage.Mutate(x => x.DrawImage(image, new Point(0, 0), 1f).DrawImage(overlayImage, new Point(0, 0), 1f));
                }

                var memoryStream = new MemoryStream();
                var encoder = new JpegEncoder { Quality = _mediaSettings.DefaultImageQuality };
                if (finalImage != null)
                {
                    await finalImage.SaveAsync(memoryStream, encoder);
                }
                else
                {
                    await image.SaveAsync(memoryStream, encoder);
                }

                memoryStream.Position = 0;

                return File(memoryStream, mimeTypeString);
            }
            finally
            {
                finalImage?.Dispose();
                overlayImage?.Dispose();
                image.Dispose();
            }
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
