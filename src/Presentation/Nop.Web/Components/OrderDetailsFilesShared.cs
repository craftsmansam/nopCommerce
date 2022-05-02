using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

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
