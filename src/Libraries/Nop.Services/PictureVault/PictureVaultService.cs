using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.PictureVault;
using Nop.Data;

namespace Nop.Services.PictureVault
{
    /// <summary>
    /// Affiliate service
    /// </summary>
    public partial class PictureVaultService : IPictureVaultService
    {
        private readonly INopDataProvider _dataProvider;

        public PictureVaultService(INopDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<Tuple<string, DateTime, string>> CustomerListPurchaseOrders(int? salesContactId)
        {
            var listOfCustomerListPurchaseOrders = GetCustomerListPurchaseOrders(salesContactId);
            var purchaseOrderList = listOfCustomerListPurchaseOrders.Select(x =>
                new Tuple<string, DateTime, string>(string.IsNullOrEmpty(x.PurchaseOrder) ? "-blank" : x.PurchaseOrder,
                    x.DateTimeUploaded, string.IsNullOrEmpty(x.ProjectName) ? null : x.ProjectName + " "));
            return purchaseOrderList.ToList();
        }

        private IList<SpShopOrder_ListPurchaseOrders> GetCustomerListPurchaseOrders(int? salesContactId)
        {
            //test this id first: 276858
            var salesContactIdParameter = SqlParameterHelper.GetInt32Parameter("salesContactId", salesContactId);
            return _dataProvider.QueryProc<SpShopOrder_ListPurchaseOrders>("ShopOrder_ListPurchaseOrders", salesContactIdParameter);
        }
        
        public IList<PictureVaultItem> CustomerListPictureVaultItems(int? salesContactId, string po)
        {
            var salesContactIdParameter = SqlParameterHelper.GetInt32Parameter("salesContactID", salesContactId);
            var poParameter = SqlParameterHelper.GetStringParameter("customerPurchaseOrder", po);
            return _dataProvider.QueryProc<PictureVaultItem>("Customer_ListPictureVaultItems", salesContactIdParameter, poParameter);
        }

        public bool IsPictureForMyCompany(int customerSalesContactId, int imageId)
        {
            var salesContactIdParameter = SqlParameterHelper.GetInt32Parameter("salesContactID", customerSalesContactId);
            var shopOrderPictureIdParameter = SqlParameterHelper.GetInt32Parameter("shopOrderPictureID", imageId);
            return _dataProvider.QueryProc<bool>("CustomerSalesContact_AssertPictureIsForMyCompany", salesContactIdParameter, shopOrderPictureIdParameter).SingleOrDefault();
        }

        public string PictureVaultFileNameByShopOrderPictureId(int imageId)
        {
            var shopOrderPictureIdParameter = SqlParameterHelper.GetInt32Parameter("shopOrderPictureID", imageId);
            return _dataProvider.Query<string>("select Filename from albina.ShopOrderPicture WHERE ShopOrderPictureID = @shopOrderPictureID", shopOrderPictureIdParameter).SingleOrDefault();
        }

        public int PictureVaultSONumberBySOPictureId(int imageId)
        {
            var shopOrderPictureIdParameter = SqlParameterHelper.GetInt32Parameter("shopOrderPictureID", imageId);
            return _dataProvider.Query<int>("select ShopOrderNumber from albina.ShopOrderPicture WHERE ShopOrderPictureID = @shopOrderPictureID", shopOrderPictureIdParameter).SingleOrDefault();
        }
    }
}