using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;

namespace Nop.Services.Orders
{
    /// <summary>
    /// Order service interface
    /// </summary>
    public partial interface IShopOrderService
    {
        /// <summary>
        /// Lists shop orders for a given customer
        /// </summary>
        /// <param name="customerID">The Job Costing customerID</param>
        /// <returns>List of ShopOrder</returns>
        Task<List<ShopOrder>> ListShopOrdersForCustomerAsync(int customerID);

        Task<ShopOrder> GetShopOrderByShopOrderNumberAsync(int shopOrderNumber);
        bool TestMTRPermissions(int mtrId, int? jcCustomerID, out ShopOrderMTR mtr, out ShopOrder shopOrder);
        bool TestShopOrderPermissions(int shopOrderId, int? jcCustomerID, out ShopOrder shopOrder);
    }
}