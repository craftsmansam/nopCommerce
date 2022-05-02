using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Quotes;
using Nop.Web.Factories;
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
        public virtual IActionResult CustomerQuotes()
        {
            if (!_customerService.IsRegistered(_workContext.CurrentCustomer))
                return Challenge();
            
            var model = _quoteModelFactory.PrepareCustomerQuoteListModel();
            
            return View(model);
        }

        [HttpsRequirement]
        public virtual IActionResult QuoteDetails(int quoteId)
        {
            var quote = _quoteService.GetQuoteByQuoteId(quoteId);
            if (quote == null || _workContext.CurrentCustomer.JCCustomerID != quote.CustomerID)
                return Challenge();

            var model = _quoteModelFactory.PrepareQuoteDetailsModel(quote);
            return View(model);
        }

        [HttpsRequirement]
        public virtual IActionResult QuoteReport(int quoteId)
        {
            if (!_quoteService.TestQuotePermissions(quoteId, _workContext.CurrentCustomer.JCCustomerID, out var quote))
                return AccessDeniedView();

            return ServeWebAttachment(_settingService, quote.QuoteReportFilename);
        }
    }
}
