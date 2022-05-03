using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Order
{
    public partial record CustomerQuoteListModel : BaseNopModel
    {
        public IList<QuoteDetailsModel> Quotes { get; set; } = new List<QuoteDetailsModel>();
    }
}