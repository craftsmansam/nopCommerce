using LinqToDB;
using LinqToDB.Data;
using Nop.Data;

namespace Nop.Services.AlbinaUnsubscribe
{
    /// <summary>
    /// Unsubscribe service
    /// </summary>
    public class AlbinaUnsubscribeService : IUnsubscribeService
    {
        private readonly INopDataProvider _dataProvider;

        public AlbinaUnsubscribeService(INopDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IList<string>> FlagAsDoNotEmailAsync(string unsubscribeType, int customerOrVendorContactID, int? bulkEmailID)
        {
            var salesContactIdParameter = new DataParameter("@ID", customerOrVendorContactID, DataType.Int32);
            var bulkEmailIDParameter = new DataParameter("@bulkEmailID", bulkEmailID, DataType.Int32);
            var storedProcName = unsubscribeType == "customer" ? "Unsubscribe_Customer" : "Unsubscribe_Vendor";
            return await _dataProvider.QueryProcAsync<string>(storedProcName, salesContactIdParameter, bulkEmailIDParameter);

        }

        public async Task<IList<string>> FlagAsDoNotEmailAsync(string emailAddress, int? bulkEmailID)
        {
            var salesContactIdParameter = new DataParameter("@EmailAddress", emailAddress, DataType.VarChar);
            var bulkEmailIDParameter = new DataParameter("@bulkEmailID", bulkEmailID, DataType.Int32);
            return await _dataProvider.QueryProcAsync<string>("Unsubscribe_Email", salesContactIdParameter, bulkEmailIDParameter);
        }
    }
}