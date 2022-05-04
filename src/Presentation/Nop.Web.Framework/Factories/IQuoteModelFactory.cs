using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Web.Framework.Models.Order;

namespace Nop.Web.Framework.Factories
{
    /// <summary>
    /// Represents the interface of the shop order model factory
    /// </summary>
    public partial interface IQuoteModelFactory
    {
        /// <summary>
        /// Prepare the shop order details model
        /// </summary>
        /// <returns>Shop order details model</returns>
        QuoteDetailsModel PrepareQuoteDetailsModel(Quote quote);

        /// <summary>
        /// Prepare the customer order list model
        /// </summary>
        /// <returns>Customer order list model</returns>
        Task<CustomerQuoteListModel> PrepareCustomerQuoteListModelAsync();
    }
}
