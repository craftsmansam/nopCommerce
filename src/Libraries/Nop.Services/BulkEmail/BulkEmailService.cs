using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Nop.Data;

namespace Nop.Services.BulkEmail
{
    public class BulkEmailService : IBulkEmailService
    {
        private readonly INopDataProvider _dataProvider;

        public BulkEmailService(INopDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<string> RegisterClickAndGetRedirectUrlAsync(Guid redirectGuid)
        {
            var guidParam = new DataParameter("bulkEmailContactLinkID", redirectGuid, DataType.Guid);
            var results = await _dataProvider.QueryProcAsync<string>("BulkEmailContactLink_RegisterClick", guidParam);
            var result = results.Count > 0 ? results.First() : null;
            return result != null && !string.IsNullOrWhiteSpace(result) ? result : null;
            
        }
    }
}