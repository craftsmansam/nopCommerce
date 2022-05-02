using System.Collections.Generic;
using Nop.Core.Domain.Orders;

namespace Nop.Services.Quotes
{
    /// <summary>
    /// Tangent material service interface
    /// </summary>
    public interface IQuoteService
    {
        /// <summary>
        /// Lists shop orders for a given customer
        /// </summary>
        /// <param name="customerID">The Job Costing customerID</param>
        /// <returns>List of ShopOrder</returns>
        List<Quote> ListQuotesForCustomer(int customerID);
        Quote GetQuoteByQuoteId(int quoteId);
        bool TestQuotePermissions(int quoteId, int? jcCustomerID, out Quote quote);
    }
}