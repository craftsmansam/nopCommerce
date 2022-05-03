using Nop.Core.Domain.Calculators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Services.Calculators
{
    /// <summary>
    /// Tangent material service interface
    /// </summary>
    public partial interface ITangentMaterialService
    {    
         /// <summary>
        /// Gets all Tangent Materials material Names
        /// </summary>
        /// <returns>Categories</returns>
        Task<List<string>> GetAllUniqueMaterialNamesAsync();
         /// <summary>
        /// Gets all Tangent Materials material sizes
        /// </summary>
        /// <returns>Categories</returns>
        Task<List<string>> GetAllMaterialSizesAsync();
        Task<IList<VwTangentMaterial>> GetTangentTableAsync();

        Task<string> GetTangentMaterialResultAsync(string bendType, string materialName, string materialSize);
    }
}