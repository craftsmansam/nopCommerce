using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Data;

namespace Nop.Services.Quotes
{
    /// <summary>
    /// Affiliate service
    /// </summary>
    public partial class QuoteService : IQuoteService
    {
        private readonly IRepository<Quote> _quoteRepository;
        private readonly INopDataProvider _dataProvider;

        public QuoteService(IRepository<Quote> quoteRepository, INopDataProvider dataProvider)
        {
            _quoteRepository = quoteRepository;
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Lists quotes for a given customer
        /// </summary>
        /// <param name="jobCostingCustomerID">The Job Costing customerID</param>
        /// <returns>Order</returns>
        public virtual List<Quote> ListQuotesForCustomer(int jobCostingCustomerID)
        {
            if (jobCostingCustomerID == 0)
                return null;

            var query = _quoteRepository.Table;
            query = query.Where(x => x.CustomerID == jobCostingCustomerID);

            return query.ToList();
        }

        public Quote GetQuoteByQuoteId(int quoteId)
        {
            if (quoteId == 0)
                return null;

            var quote = _quoteRepository.Table.SingleOrDefault(x => x.Id== quoteId);
            return quote;
        }

        public bool TestQuotePermissions(int quoteId, int? jcCustomerID, out Quote quote)
        {
            quote = _quoteRepository.Table.SingleOrDefault(x => x.Id == quoteId);
            if (jcCustomerID != null && quote != null && quote.CustomerID == jcCustomerID.Value)
            {
                return true;
            }

            quote = null;
            return false;
        }
    }
}