using LinqToDB.Mapping;

namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents a shop order
    /// </summary>
    [Table("ShopOrder", Schema = "albina")]
    public partial class ShopOrder : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the shop order number
        /// </summary>
        public int ShopOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the purchase order
        /// </summary>
        public string PurchaseOrder { get; set; }

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the customer ID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Gets or sets the order date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date RTS
        /// </summary>
        public DateTime? DateRTS { get; set; }

        /// <summary>
        /// Gets or sets the date shipped
        /// </summary>
        public DateTime? DateShipped { get; set; }

        public decimal QuotedPrice { get; set; }

        public string ShopOrderConfirmationFilename { get; set; }

        public OrderStatus NopOrderStatus
        {
            get
            {
                switch (Status)
                {
                    case "In Process":
                        return OrderStatus.Processing;
                    case "Canceled":
                        return OrderStatus.Cancelled;
                    case "On-Hold":
                        return OrderStatus.OnHold;
                    case "Ready to ship":
                        return OrderStatus.Processing;
                    case "Shipped":
                        return OrderStatus.Complete;
                    default:
                        return OrderStatus.Ordered;
                }
            }
        }

        #endregion
    }

    [Table("ShopOrderMTR", Schema = "albina")]
    public partial class ShopOrderMTR : BaseEntity
    {
        public int ShopOrderNumber { get; set; }
        public string FilenameOnDisk { get; set; }
    }
}