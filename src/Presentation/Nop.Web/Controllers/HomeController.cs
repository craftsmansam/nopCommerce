using System;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers;

public partial class HomeController : BasePublicController
{
    public virtual IActionResult Index()
    {
        return View();
    }

    public IActionResult Throw()
    {
            throw new ApplicationException("We've thrown from a URL request!");
        
    }
}