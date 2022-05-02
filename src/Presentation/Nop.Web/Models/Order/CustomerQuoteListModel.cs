using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Order
{
    public partial class CustomerQuoteListModel : BaseNopModel
    {
        public CustomerQuoteListModel()
        {
            Quotes = new List<QuoteDetailsModel>();
        }

        public IList<QuoteDetailsModel> Quotes { get; set; }
    }
}