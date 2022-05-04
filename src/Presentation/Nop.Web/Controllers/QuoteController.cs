using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Quotes;
using Nop.Web.Factories;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class QuoteController : BaseWebAttachmentPublicController
    {
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IQuoteService _quoteService;
        private readonly IQuoteModelFactory _quoteModelFactory;
        private readonly ISettingService _settingService;

        public QuoteController(IWorkContext workContext, IQuoteService quoteService, ICustomerService customerService, IQuoteModelFactory quoteModelFactory, ISettingService settingService)
        {
            _customerService = customerService;
            _workContext = workContext;
            _quoteService = quoteService;
            _quoteModelFactory = quoteModelFactory;
            _settingService = settingService;
        }

        [HttpsRequirement]
        public async virtual Task<IActionResult> CustomerQuotes()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (!(await _customerService.IsRegisteredAsync(customer)))
                return Challenge();
            
            var model = await _quoteModelFactory.PrepareCustomerQuoteListModelAsync();
            
            return View(model);
        }

        [HttpsRequirement]
        public async virtual Task<IActionResult> QuoteDetails(int quoteId)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var quote = _quoteService.GetQuoteByQuoteId(quoteId);

            if (quote == null || customer.JCCustomerID != quote.CustomerID)
                return Challenge();

            var model = _quoteModelFactory.PrepareQuoteDetailsModel(quote);
            return View(model);
        }

        [HttpsRequirement]
        public virtual async Task<IActionResult> QuoteReport(int quoteId)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (!_quoteService.TestQuotePermissions(quoteId, customer.JCCustomerID, out var quote))
                return AccessDeniedView();

            return await ServeWebAttachmentAsync(_settingService, quote.QuoteReportFilename);
        }
    }
}
