using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nop.Core.Domain.Calculators
{
    /// <summary>
    /// Address
    /// </summary>
    public partial class TangentMaterial : BaseEntity
    {
        /// <summary>
        /// Override the default Id to be custom TangentMaterialID value
        /// </summary>
        [NotMapped]
        public new int Id { get => TangentMaterialID; set => TangentMaterialID = value; }
        /// <summary>
        /// Gets or sets the Tangent Material Id
        /// </summary>
        public int TangentMaterialID {get; set;}

        /// <summary>
        /// Gets or sets the material name
        /// </summary>
        public string MaterialName { get; set; } 

        /// <summary>
        /// Gets or sets the tangent material size range id
        /// </summary>
        public int TangentMaterialSizeRangeID { get; set; } 

        /// <summary>
        /// Gets or sets the bend type
        /// </summary>
        public string BendType { get; set; } 

        /// <summary>
        /// Gets or sets the tangent
        /// </summary>
        public string Tangent { get; set; } 

        /// <summary>
        /// Gets or sets the sort order
        /// </summary>
        public int SortOrder { get; set; } 
    }
}
