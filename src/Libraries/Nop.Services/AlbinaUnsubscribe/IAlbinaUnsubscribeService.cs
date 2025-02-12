namespace Nop.Services.AlbinaUnsubscribe
{
    public interface IUnsubscribeService
    {
        Task<IList<string>> FlagAsDoNotEmailAsync(string unsubscribeType, int customerOrVendorContactID, int? bulkEmailID);
        Task<IList<string>> FlagAsDoNotEmailAsync(string emailAddress, int? bulkEmailID);
    }
}