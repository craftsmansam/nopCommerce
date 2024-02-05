using System;
using System.Linq;
using System.Threading.Tasks;
using Craftsman.Mail;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.AlbinaUnsubscribe;

namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AlbinaUnsubscribeController : BasePublicController
    {
        private readonly IUnsubscribeService _unsubscribeService;

        public AlbinaUnsubscribeController(IUnsubscribeService unsubscribeService)
        {
            _unsubscribeService = unsubscribeService;
        }

        public async Task<IActionResult> UnsubscribeAsync(string unsubscribeType, string thingToUnsubscribe)
        {
            unsubscribeType = unsubscribeType.ToLower();
            if (unsubscribeType == "customer")
            {
                if (int.TryParse(thingToUnsubscribe, out var customerSalesContactID))
                {
                    var emailAddressList = await _unsubscribeService.FlagAsDoNotEmailAsync(unsubscribeType, customerSalesContactID);
                    var emailAddress = emailAddressList.Single();
                    return View("Unsubscribe", emailAddress);
                }
            }
            else if (unsubscribeType == "vendor")
            {
                if (int.TryParse(thingToUnsubscribe, out var vendorContactID))
                {
                    var emailAddressList = await _unsubscribeService.FlagAsDoNotEmailAsync(unsubscribeType, vendorContactID);
                    var emailAddress = emailAddressList.Single();
                    return View("Unsubscribe", emailAddress);
                }
            }
            else if (unsubscribeType == "email")
            {
                if (EmailHelper.IsValidEmail(thingToUnsubscribe))
                {
                    var emailAddressList = await _unsubscribeService.FlagAsDoNotEmailAsync(unsubscribeType, thingToUnsubscribe);
                    var emailAddress = emailAddressList.Single();
                    return View("Unsubscribe", emailAddress);
                }
            }

            throw new NotImplementedException($"Unsubscribe type of {unsubscribeType} with value {thingToUnsubscribe} is not supported");
        }
    }
}
