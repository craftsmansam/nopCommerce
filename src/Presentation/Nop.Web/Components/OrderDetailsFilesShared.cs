using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Models.Order;

namespace Nop.Web.Components
{
    public class OrderDetailsFilesSharedViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(IOrderDetailsModel model)
        {
            return View(model);
        }

    }

}
