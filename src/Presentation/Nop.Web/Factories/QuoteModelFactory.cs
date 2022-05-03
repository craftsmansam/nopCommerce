using System;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Quotes;
using Nop.Web.Models.Order;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the order model factory
    /// </summary>
    public partial class QuoteModelFactory : IQuoteModelFactory
    {
        private readonly IWorkContext _workContext;
        private readonly IQuoteService _quoteService;

        public QuoteModelFactory(IWorkContext workContext, IQuoteService quoteService)
        {
            _workContext = workContext;
            _quoteService = quoteService;
        }

        #region Methods

        /// <summary>
        /// Prepare the quote details model
        /// </summary>
        /// <param name="quote">Quote</param>
        /// <returns>Quote details model</returns>
        public virtual QuoteDetailsModel PrepareQuoteDetailsModel(Quote quote)
        {
            if (quote == null)
                throw new ArgumentNullException(nameof(quote));

            var model = new QuoteDetailsModel
            {
                Id = quote.Id,
                QuoteDate = quote.QuoteDate,
                ProjectName = quote.ProjectName,
                CustomerContact = quote.CustomerContact,
                QuoteNumber = quote.QuoteNumber,
                QuoteReportFilename = quote.QuoteReportFilename
            };

            return model;
        }

        public async Task<CustomerQuoteListModel> PrepareCustomerQuoteListModelAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var model = new CustomerQuoteListModel();
            if (customer.JCCustomerID.HasValue)
            {
                var quotes = _quoteService.ListQuotesForCustomer(customer.JCCustomerID.Value);
            
                foreach (var quote in quotes)
                {
                    var quoteModel = new QuoteDetailsModel
                    {
                        Id = quote.Id,
                        QuoteDate = quote.QuoteDate,
                        ProjectName = quote.ProjectName,
                        CustomerContact = quote.CustomerContact,
                        QuoteNumber = quote.QuoteNumber
                    };

                    model.Quotes.Add(quoteModel);
                }
            }

            return model;
        }

        #endregion
    }
}