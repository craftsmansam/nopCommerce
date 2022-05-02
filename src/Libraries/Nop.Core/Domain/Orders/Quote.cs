using System;
using LinqToDB.Mapping;

namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents a shop order
    /// </summary>
    [Table("Quote", Schema = "albina")]
    public partial class Quote : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the quote number
        /// </summary>
        public string QuoteNumber { get; set; }

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the customer ID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Gets or sets the quote date
        /// </summary>
        public DateTime QuoteDate { get; set; }

        /// <summary>
        /// Gets or sets the customer contact
        /// </summary>
        public string CustomerContact { get; set; }

        public string QuoteReportFilename { get; set; }

        #endregion
    }
}