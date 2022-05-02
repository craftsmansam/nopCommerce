using System;
using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Order
{
    public partial class QuoteDetailsModel : BaseNopEntityModel
    {
        public bool PrintMode { get; set; }

        public string QuoteNumber { get; set; }
        public DateTime QuoteDate { get; set; }
        public string CustomerContact { get; set; }
        public string ProjectName { get; set; }
        public string QuoteReportFilename { get; set; }
    }
}