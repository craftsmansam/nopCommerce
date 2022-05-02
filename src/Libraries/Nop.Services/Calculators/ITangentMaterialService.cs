using Nop.Core.Domain.Calculators;
using System.Collections.Generic;

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
        List<string> GetAllUniqueMaterialNames();
         /// <summary>
        /// Gets all Tangent Materials material sizes
        /// </summary>
        /// <returns>Categories</returns>
        List<string> GetAllMaterialSizes();
        IList<VwTangentMaterial> GetTangentTable();

        string GetTangentMaterialResult(string bendType, string materialName, string materialSize);
    }
}