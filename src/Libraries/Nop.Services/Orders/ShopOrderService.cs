using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Data;

namespace Nop.Services.Orders
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial class ShopOrderService : IShopOrderService
    {
        #region Fields

        private readonly IRepository<ShopOrder> _shopOrderRepository;
        private readonly IRepository<ShopOrderMTR> _shopOrderMtrRepository;
        private readonly INopDataProvider _dataProvider;

        #endregion

        #region Ctor

        public ShopOrderService(IRepository<ShopOrder> shopOrderRepository, IRepository<ShopOrderMTR> shopOrderMtrRepository, INopDataProvider dataProvider)
        {
            _shopOrderRepository = shopOrderRepository;
            _shopOrderMtrRepository = shopOrderMtrRepository;
            _dataProvider = dataProvider;
        }

        #endregion

        #region Methods

        #region Orders

        /// <summary>
        /// Lists shop orders for a given customer
        /// </summary>
        /// <param name="customerID">The Job Costing customerID</param>
        /// <returns>Order</returns>
        public virtual List<ShopOrder> ListShopOrdersForCustomer(int customerID)
        {
            if (customerID == 0)
                return null;

            var query = _shopOrderRepository.Table;
            query = query.Where(x => x.CustomerID == customerID);

            return query.ToList();
        }

        public ShopOrder GetShopOrderByShopOrderNumber(int shopOrderNumber)
        {
            if (shopOrderNumber == 0)
                return null;

            var shopOrder = _shopOrderRepository.Table.SingleOrDefault(x => x.ShopOrderNumber == shopOrderNumber);
            return shopOrder;
        }

        public bool TestMTRPermissions(int mtrId, int? jcCustomerID, out ShopOrderMTR mtr, out ShopOrder shopOrder)
        {
            mtr = _shopOrderMtrRepository.Table.SingleOrDefault(x => x.Id == mtrId);
            if (jcCustomerID != null && mtr != null)
            {
                var shopOrderNumber = mtr.ShopOrderNumber;
                shopOrder = _shopOrderRepository.Table.SingleOrDefault(x => x.ShopOrderNumber == shopOrderNumber);
                if (shopOrder != null && shopOrder.CustomerID == jcCustomerID.Value)
                {
                    return true;
                }
            }

            mtr = null;
            shopOrder = null;
            return false;
        }

        public bool TestShopOrderPermissions(int shopOrderId, int? jcCustomerID, out ShopOrder shopOrder)
        {
            shopOrder = _shopOrderRepository.Table.SingleOrDefault(x => x.Id == shopOrderId);
            if (jcCustomerID != null && shopOrder != null && shopOrder.CustomerID == jcCustomerID.Value)
            {
                return true;
            }

            shopOrder = null;
            return false;
        }
        #endregion
        
        #endregion
    }
}