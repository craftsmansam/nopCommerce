using System.Collections.Generic;
using Nop.Core.Domain.Calculators;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Common
{
    public partial class BendingTolerancesTableModel : BaseNopModel
    {
        public BendingTolerancesTableModel()
        {
           BTRows = new List<BendingTolerancesRow>();
        }

        public List<BendingTolerancesRow> BTRows {get; set;}

		public bool ShowFooter { get; set; }

		public string TableDescription { get; set; }

        public string HotOrCold { get; set; }
    }
}