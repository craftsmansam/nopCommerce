﻿using System;

namespace Nop.Web.Framework.Models.Order
{
    public partial record QuoteDetailsModel : BaseNopEntityModel
    {
        public bool PrintMode { get; set; }

        public string QuoteNumber { get; set; }
        public DateTime QuoteDate { get; set; }
        public string CustomerContact { get; set; }
        public string ProjectName { get; set; }
        public string QuoteReportFilename { get; set; }
    }
}