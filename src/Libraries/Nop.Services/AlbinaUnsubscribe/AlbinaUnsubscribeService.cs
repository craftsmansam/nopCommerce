using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IList<string>> FlagAsDoNotEmailAsync(string unsubscribeType, int customerOrVendorContactID)
        {
            var salesContactIdParameter = new DataParameter("ID", customerOrVendorContactID, DataType.Int32);
            var storedProcName = unsubscribeType == "customer" ? "Unsubscribe_Customer" : "Unsubscribe_Vendor";
            return await _dataProvider.QueryProcAsync<string>(storedProcName, salesContactIdParameter);

        }

        public async Task<IList<string>> FlagAsDoNotEmailAsync(string unsubscribeType, string emailAddress)
        {
            var salesContactIdParameter = new DataParameter("EmailAddress", emailAddress, DataType.VarChar);
            return await _dataProvider.QueryProcAsync<string>("Unsubscribe_Email", salesContactIdParameter);
        }
    }
}