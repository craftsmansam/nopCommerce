using System.Collections.Generic;
using Nop.Core.Domain.Calculators;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Common
{
    public enum BendToleranceType
    {
        ColdStringerLessThan12,
        ColdStringerGreaterThan12,
        HotStringerLessThan20,
        HotStringerGreaterThan20,
        ColdHandrailLessThan12,
        ColdHandrailGreaterThan12
    }

    public record BendingTolerancesTableModel : BaseNopModel
    {
        public List<BendingTolerancesRow> BendToleranceRows { get; set; } = new();

		public bool ShowFooter { get; set; }

		public string TableDescription { get; set; }

        public string HotOrCold { get; set; }
    }
}