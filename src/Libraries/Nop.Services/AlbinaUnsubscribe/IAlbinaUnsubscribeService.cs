using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Services.AlbinaUnsubscribe
{
    public interface IUnsubscribeService
    {
        Task<IList<string>> FlagAsDoNotEmailAsync(string unsubscribeType, int customerOrVendorContactID);
        Task<IList<string>> FlagAsDoNotEmailAsync(string unsubscribeType, string emailAddress);
    }
}