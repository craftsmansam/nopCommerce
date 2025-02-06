namespace Nop.Web.Framework.Models.Order;

/// <summary>
/// Represents an order details common model
/// </summary>
public interface IOrderDetailsModel
{
    public DateTime OrderDate { get; set; }
    public string PurchaseOrder { get; }
    public string ProjectName { get; }
    public string Status { get; set; }
    public DateTime? DateRTS { get; }
    public DateTime? DateShipped { get; }
    public string OrderTotal { get; }
    public string ShopOrderConfirmationFilename { get; set; }
    public List<int> MtrDocumentIDs { get; set; }
    public int ShopOrderId { get; set; }
}