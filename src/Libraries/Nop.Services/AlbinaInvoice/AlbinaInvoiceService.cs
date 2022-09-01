using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Nop.Core.Domain.AlbinaInvoice;
using Nop.Data;

namespace Nop.Services.AlbinaInvoice
{
    /// <summary>
    /// Affiliate service
    /// </summary>
    public class AlbinaInvoiceService : IAlbinaInvoiceService
    {
        private readonly INopDataProvider _dataProvider;

        public AlbinaInvoiceService(INopDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IList<AlbinaInvoiceItem>> CustomerListInvoicesAsync(int? salesContactId)
        {
            var salesContactIdParameter = new DataParameter("salesContactId", salesContactId, DataType.Int32);
            return await _dataProvider.QueryProcAsync<AlbinaInvoiceItem>("CustomerSalesContact_ListContentCentralInvoices", salesContactIdParameter);
        }
        
        public async Task<bool> IsInvoiceForMyCompanyAsync(int customerSalesContactId, int shopOrderContentCentralInvoiceId)
        {
            var salesContactIdParameter = new DataParameter("salesContactId", customerSalesContactId, DataType.Int32);
            var shopOrderContentCentralInvoiceIdParameter = new DataParameter("shopOrderContentCentralInvoiceID", shopOrderContentCentralInvoiceId, DataType.Int32);
            return (await _dataProvider.QueryProcAsync<bool>("CustomerSalesContact_AssertInvoiceIsForMyCompany", salesContactIdParameter, shopOrderContentCentralInvoiceIdParameter)).SingleOrDefault();
        }

        public async Task<string> InvoiceFileNameByShopOrderContentCentralInvoiceIdAsync(int shopOrderContentCentralInvoiceID)
        {
            var shopOrderContentCentralInvoiceIdParameter = new DataParameter("shopOrderContentCentralInvoiceID", shopOrderContentCentralInvoiceID, DataType.Int32);
            return (await _dataProvider.QueryAsync<string>("select DocumentPath + DocumentFilename from albina.ShopOrderContentCentralInvoice WHERE ShopOrderContentCentralInvoiceID = @shopOrderContentCentralInvoiceID", shopOrderContentCentralInvoiceIdParameter)).SingleOrDefault();
        }
    }
}