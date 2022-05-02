using System.Collections.Generic;
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
        List<ShopOrder> ListShopOrdersForCustomer(int customerID);

        ShopOrder GetShopOrderByShopOrderNumber(int shopOrderNumber);
        bool TestMTRPermissions(int mtrId, int? jcCustomerID, out ShopOrderMTR mtr, out ShopOrder shopOrder);
        bool TestShopOrderPermissions(int shopOrderId, int? jcCustomerID, out ShopOrder shopOrder);
    }
}