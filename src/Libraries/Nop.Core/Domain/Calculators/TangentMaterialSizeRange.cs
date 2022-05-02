using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nop.Core.Domain.Calculators
{
    /// <summary>
    /// Address
    /// </summary>
    public partial class TangentMaterialSizeRange : BaseEntity
    {
        /// <summary>
        /// Override the default Id to be custom TangentMaterialID value
        /// </summary>
        [NotMapped]
        public new int Id { get => TangentMaterialSizeRangeID; set => TangentMaterialSizeRangeID = value; }
        /// <summary>
        /// Gets or sets the Tangent Material Size Range Id
        /// </summary>
        public int TangentMaterialSizeRangeID {get; set;}
        /// <summary>
        /// Gets or sets the material size
        /// </summary>
        public string MaterialSize {get; set;}
        /// <summary>
        /// Gets or sets the Sort Order
        /// </summary>
        public int SortOrder{get; set;}


        
    }
}
