﻿using Nop.Core.Domain.Orders;
using Nop.Web.Models.Order;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the interface of the shop order model factory
    /// </summary>
    public partial interface IShopOrderModelFactory
    {
        /// <summary>
        /// Prepare the shop order details model
        /// </summary>
        /// <returns>Shop order details model</returns>
        ShopOrderDetailsModel PrepareShopOrderDetailsModel(ShopOrder shopOrder);
    }
}
