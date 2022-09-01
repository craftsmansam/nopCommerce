using System;

namespace Nop.Web.Models.AlbinaInvoice
{
    public class AlbinaInvoiceModel
    {
        public int ShopOrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ShowInvoiceUrl { get; set; }

        public AlbinaInvoiceModel(int shopOrderNumber, string invoiceNumber, DateTime invoiceDate, string showInvoiceUrl)
        {
            ShopOrderNumber = shopOrderNumber;
            InvoiceNumber = invoiceNumber;
            InvoiceDate = invoiceDate;
            ShowInvoiceUrl = showInvoiceUrl;
        }
    }
}