using System;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Web.Framework.Models.Order;

namespace Nop.Web.Framework.Factories
{
    /// <summary>
    /// Represents the order model factory
    /// </summary>
    public partial class ShopOrderModelFactory : IShopOrderModelFactory
    {
        private readonly IRepository<ShopOrderMTR> _shopOrderMtrRepository;

        public ShopOrderModelFactory(IRepository<ShopOrderMTR> shopOrderMtrRepository)
        {
            _shopOrderMtrRepository = shopOrderMtrRepository;
        }

        #region Methods

        /// <summary>
        /// Prepare the shop order details model
        /// </summary>
        /// <param name="shopOrder">Shop Order</param>
        /// <returns>Shop Order details model</returns>
        public virtual ShopOrderDetailsModel PrepareShopOrderDetailsModel(ShopOrder shopOrder)
        {
            if (shopOrder == null)
                throw new ArgumentNullException(nameof(shopOrder));

            var query = _shopOrderMtrRepository.Table.Where(x => x.ShopOrderNumber == shopOrder.ShopOrderNumber);

            var model = new ShopOrderDetailsModel
            {
                Id = shopOrder.Id,
                OrderDate = shopOrder.OrderDate,
                Status = shopOrder.Status,
                ShopOrderNumber = shopOrder.ShopOrderNumber,
                DateShipped = shopOrder.DateShipped,
                DateRTS = shopOrder.DateRTS, 
                PurchaseOrder = shopOrder.PurchaseOrder,
                ProjectName = shopOrder.ProjectName,
                QuotedPrice = shopOrder.QuotedPrice,
                ShopOrderConfirmationFilename = shopOrder.ShopOrderConfirmationFilename,

                MtrDocumentIDs = query.Select(x => x.Id).ToList()
            };

            return model;
        }

        #endregion
    }
}