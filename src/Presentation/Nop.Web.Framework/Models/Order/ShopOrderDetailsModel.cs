using Craftsman.Data;

namespace Nop.Web.Framework.Models.Order
{
    public partial record ShopOrderDetailsModel : BaseNopEntityModel, IOrderDetailsModel
    {
        public bool PrintMode { get; set; }

        public int ShopOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DateRTS { get; set; }
        public DateTime? DateShipped { get; set; }
        public string Status { get; set; }
        public string PurchaseOrder { get; set; }
        public string ProjectName { get; set; }
        public decimal QuotedPrice { get; set; }
        public string ShopOrderConfirmationFilename { get; set; }

        public List<int> MtrDocumentIDs { get; set; }

        public string OrderTotal => QuotedPrice.ToString(FormatStrings.CurrencyFormat);

        public int ShopOrderId
        {
            get => Id;
            set => Id = value;
        }
    }
}