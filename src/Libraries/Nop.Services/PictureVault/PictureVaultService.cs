using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
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

        public async Task<List<Tuple<string, DateTime, string>>> CustomerListPurchaseOrdersAsync(int? salesContactId)
        {
            var listOfCustomerListPurchaseOrders = await GetCustomerListPurchaseOrdersAsync(salesContactId);
            var purchaseOrderList = listOfCustomerListPurchaseOrders.Select(x =>
                new Tuple<string, DateTime, string>(string.IsNullOrEmpty(x.PurchaseOrder) ? "-blank" : x.PurchaseOrder,
                    x.DateTimeUploaded, string.IsNullOrEmpty(x.ProjectName) ? null : x.ProjectName + " "));
            return purchaseOrderList.ToList();
        }

        private async Task<IList<SpShopOrder_ListPurchaseOrders>> GetCustomerListPurchaseOrdersAsync(int? salesContactId)
        {
            //test this id first: 276858
            var salesContactIdParameter = new DataParameter("salesContactId", salesContactId, DataType.Int32);
            return await _dataProvider.QueryProcAsync<SpShopOrder_ListPurchaseOrders>("ShopOrder_ListPurchaseOrders", salesContactIdParameter);
        }
        
        public async Task<IList<PictureVaultItem>> CustomerListPictureVaultItemsAsync(int? salesContactId, string po)
        {
            var salesContactIdParameter = new DataParameter("salesContactId", salesContactId, DataType.Int32);
            var poParameter = new DataParameter("customerPurchaseOrder", po, DataType.NVarChar);
            return await _dataProvider.QueryProcAsync<PictureVaultItem>("Customer_ListPictureVaultItems", salesContactIdParameter, poParameter);
        }

        public async Task<bool> IsPictureForMyCompanyAsync(int customerSalesContactId, int imageId)
        {
            var salesContactIdParameter = new DataParameter("salesContactId", customerSalesContactId, DataType.Int32);
            var shopOrderPictureIdParameter = new DataParameter("shopOrderPictureID", imageId, DataType.Int32);
            return (await _dataProvider.QueryProcAsync<bool>("CustomerSalesContact_AssertPictureIsForMyCompany", salesContactIdParameter, shopOrderPictureIdParameter)).SingleOrDefault();
        }

        public async Task<string> PictureVaultFileNameByShopOrderPictureIdAsync(int imageId)
        {
            var shopOrderPictureIdParameter = new DataParameter("shopOrderPictureID", imageId, DataType.Int32);
            return (await _dataProvider.QueryAsync<string>("select Filename from albina.ShopOrderPicture WHERE ShopOrderPictureID = @shopOrderPictureID", shopOrderPictureIdParameter)).SingleOrDefault();
        }

        public async Task<int> PictureVaultSONumberBySOPictureIdAsync(int imageId)
        {
            var shopOrderPictureIdParameter = new DataParameter("shopOrderPictureID", imageId, DataType.Int32);
            return (await _dataProvider.QueryAsync<int>("select ShopOrderNumber from albina.ShopOrderPicture WHERE ShopOrderPictureID = @shopOrderPictureID", shopOrderPictureIdParameter)).SingleOrDefault();
        }
    }
}