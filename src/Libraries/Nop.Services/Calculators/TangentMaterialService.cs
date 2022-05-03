using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Nop.Core.Domain.Calculators;
using Nop.Data;

namespace Nop.Services.Calculators
{
    /// <summary>
    /// Affiliate service
    /// </summary>
    public partial class TangentMaterialService : ITangentMaterialService
    {

        private readonly IRepository<TangentMaterial> _tangentMaterialRepository;
        private readonly IRepository<TangentMaterialSizeRange> _tangentMaterialSizeRangeRepository;
        private readonly INopDataProvider _dataProvider;

        public TangentMaterialService(IRepository<TangentMaterial> tangentMaterialRepository,
            IRepository<TangentMaterialSizeRange> tangentMaterialSizeRangeRepository,
            INopDataProvider dataProvider)
        {
            _tangentMaterialRepository = tangentMaterialRepository;
            _tangentMaterialSizeRangeRepository = tangentMaterialSizeRangeRepository;
            _dataProvider = dataProvider;
        }

         /// <summary>
        /// Gets all Tangent Materials materialNames
        /// </summary>
        /// <returns>Categories</returns>
        public virtual async Task<List<string>> GetAllUniqueMaterialNamesAsync()
        {
            var table = _tangentMaterialRepository.Table;
            return await table.OrderBy(x => x.MaterialName).Select(x => x.MaterialName).Distinct().ToListAsync();
        }

        public virtual async Task<List<string>> GetAllMaterialSizesAsync()
        {
            var table = _tangentMaterialSizeRangeRepository.Table;
            return await table.OrderBy(x => x.SortOrder).Select(x => x.MaterialSize).ToListAsync();
        }

        public virtual async Task<IList<VwTangentMaterial>> GetTangentTableAsync()
        {
            var tangentMaterials = await _dataProvider.QueryAsync<VwTangentMaterial>("select * from vwTangentMaterial order by SortOrder");
            return tangentMaterials;
        }

        public async Task<string> GetTangentMaterialResultAsync(string bendType, string materialName, string materialSize)
        {
            var materialNameParam = new DataParameter("materialName", materialName, DataType.NVarChar);
            var materialSizeParam = new DataParameter("materialSize", materialSize, DataType.NVarChar);
            var bendTypeParam = new DataParameter("bendType", bendType, DataType.NVarChar);
            var results = await _dataProvider.QueryProcAsync<string>("TangentMaterial_LookupResult", materialNameParam, materialSizeParam, bendTypeParam);
            var result = results.Count > 0 ? results.First() : null;
			return result != null && !string.IsNullOrWhiteSpace(result.ToString()) ? result + " tangents" : "not produced";
		}
    }
}