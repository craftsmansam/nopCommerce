using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Craftsman.IO;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;

namespace Nop.Web.Controllers
{
    public abstract class BaseWebAttachmentPublicController : BasePublicController
    {
        protected async Task<IActionResult> ServeWebAttachmentAsync(ISettingService settingService, string filename)
        {
            var fileExtension = Path.GetExtension(filename).Replace(".", string.Empty);
            var mimeTypeString = MimeType.All.First(x => x.FileExtensions.Any(y => y.Extension.ToLower() == fileExtension)).MimeTypeString;

            var filepath = Path.Combine((await settingService.WebAttachmentsRootFolderAsync()).DrivePath, filename);

            if (!System.IO.File.Exists(filepath))
                return AccessDeniedView();
            
            return new PhysicalFileResult(filepath, mimeTypeString);
        }
    }
}