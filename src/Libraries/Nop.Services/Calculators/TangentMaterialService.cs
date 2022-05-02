using System.Collections.Generic;
using System.Linq;
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
        public virtual List<string> GetAllUniqueMaterialNames()
        {
            var table = _tangentMaterialRepository.Table;
            return table.OrderBy(x => x.MaterialName).Select(x => x.MaterialName).Distinct().ToList();
        }

        public virtual List<string> GetAllMaterialSizes()
        {
            var table = _tangentMaterialSizeRangeRepository.Table;
            return table.OrderBy(x => x.SortOrder).Select(x => x.MaterialSize).ToList();
        }

        public virtual IList<VwTangentMaterial> GetTangentTable()
        {
            var tangentMaterials = _dataProvider.Query<VwTangentMaterial>("select * from vwTangentMaterial order by SortOrder");
            return tangentMaterials;
        }

        public string GetTangentMaterialResult(string bendType, string materialName, string materialSize)
		{
            var materialNameParam = SqlParameterHelper.GetStringParameter("materialName", materialName);
            var materialSizeParam = SqlParameterHelper.GetStringParameter("materialSize", materialSize);
            var bendTypeParam = SqlParameterHelper.GetStringParameter("bendType", bendType);
            var results = _dataProvider.QueryProc<string>("TangentMaterial_LookupResult", materialNameParam, materialSizeParam, bendTypeParam);
            var result = results.Count > 0 ? results.First() : null;
			return result != null && !string.IsNullOrWhiteSpace(result.ToString()) ? result + " tangents" : "not produced";
		}
    }
}