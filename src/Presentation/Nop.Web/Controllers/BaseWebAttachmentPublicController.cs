using System.IO;
using System.Linq;
using Craftsman.IO;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;

namespace Nop.Web.Controllers
{
    public abstract class BaseWebAttachmentPublicController : BasePublicController
    {
        protected IActionResult ServeWebAttachment(ISettingService settingService, string filename)
        {
            var fileExtension = Path.GetExtension(filename).Replace(".", string.Empty);
            var mimeTypeString = MimeType.All.First(x => x.FileExtensions.Any(y => y.Extension.ToLower() == fileExtension)).MimeTypeString;

            var filepath = Path.Combine(settingService.WebAttachmentsRootFolder().DrivePath, filename);

            if (!System.IO.File.Exists(filepath))
                return AccessDeniedView();
            
            return new PhysicalFileResult(filepath, mimeTypeString);
        }
    }
}