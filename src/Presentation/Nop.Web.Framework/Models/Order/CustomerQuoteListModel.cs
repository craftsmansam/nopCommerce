using System.Collections.Generic;

namespace Nop.Web.Framework.Models.Order
{
    public partial record CustomerQuoteListModel : BaseNopModel
    {
        public IList<QuoteDetailsModel> Quotes { get; set; } = new List<QuoteDetailsModel>();
    }
}