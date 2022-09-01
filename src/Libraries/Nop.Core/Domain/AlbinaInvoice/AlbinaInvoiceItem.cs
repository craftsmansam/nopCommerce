using System;

namespace Nop.Core.Domain.AlbinaInvoice
{
    public class AlbinaInvoiceItem
    {
        public int ShopOrderContentCentralInvoiceID { get; set; }
        public int ShopOrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}