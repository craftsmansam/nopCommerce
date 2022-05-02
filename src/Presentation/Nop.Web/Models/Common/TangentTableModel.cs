using System.Collections.Generic;
using Nop.Core.Domain.Calculators;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Common
{
    public partial class TangentTableModel : BaseNopModel
    {
        public TangentTableModel()
        {
           TangentMaterials = new List<VwTangentMaterial>();
        }

        public string BendType {get; set;}
        public string ClassPrefix { get; set; }
		public string HeaderTableID { get; set; }
		public string MainTableID { get; set; }
        public IEnumerable<VwTangentMaterial> TangentMaterials {get; set;}
    }
}