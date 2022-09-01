using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.AlbinaInvoice;

namespace Nop.Services.AlbinaInvoice
{
    public interface IAlbinaInvoiceService
    {
        Task<IList<AlbinaInvoiceItem>> CustomerListInvoicesAsync(int? salesContactId);
        Task<bool> IsInvoiceForMyCompanyAsync(int customerSalesContactId, int shopOrderContentCentralInvoiceID);
        Task<string> InvoiceFileNameByShopOrderContentCentralInvoiceIdAsync(int shopOrderContentCentralInvoiceID);
    }
}