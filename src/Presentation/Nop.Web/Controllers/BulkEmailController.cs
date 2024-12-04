using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Services.BulkEmail;

namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BulkEmailController : Controller
    {
        private readonly IBulkEmailService _bulkEmailService;

        protected readonly AlbinaConfig _albinaWebConfig;
        protected readonly IWorkContext _workContext;

        public BulkEmailController(IWorkContext context, IBulkEmailService bulkEmailService)
        {
            _albinaWebConfig = EngineContext.Current.Resolve<AlbinaConfig>();
            _workContext = context;
            _bulkEmailService = bulkEmailService;
        }


        public async Task<IActionResult> RedirectClick(Guid redirectGuid)
        {
            var redirectUrl = await _bulkEmailService.RegisterClickAndGetRedirectUrlAsync(redirectGuid);
            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return NotFound();
            }
            
            return Redirect(redirectUrl);
        }
    }
}
