using System.Collections.Generic;
using Nop.Core.Domain.Calculators;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Common
{
    public partial record BendingTolerancesTableModel : BaseNopModel
    {
        public List<BendingTolerancesRow> BTRows { get; set; } = new();

		public bool ShowFooter { get; set; }

		public string TableDescription { get; set; }

        public string HotOrCold { get; set; }
    }
}