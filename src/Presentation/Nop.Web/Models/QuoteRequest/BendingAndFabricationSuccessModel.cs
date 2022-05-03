using Nop.Web.Framework.Models;

namespace Nop.Web.Models.QuoteRequest
{
    public partial record BendingAndFabricationSuccessModel : BaseNopModel
    {
        public bool IncludeSpiral { get; set; }
    }
}