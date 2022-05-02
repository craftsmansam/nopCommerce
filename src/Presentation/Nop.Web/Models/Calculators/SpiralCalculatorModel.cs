using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Web.Models.Calculators
{
    public class SpiralCalculatorModel
    {
        [Required]
        [Display(Name = "Plan View Radius")]
        public decimal? PlanViewRadius { get; set; }

        [Display(Name = "Plan View Diameter")]
        public decimal? PlanViewDiameter { get; set; }

        [Required]
        [Display(Name = "Overall Height of Bent Section")]
        public decimal? OverallHeight { get; set; }

        [Display(Name = "Rise Height")]
        public decimal? RiseHeight { get; set; }

        [Display(Name = "No. of Rises")]
        [RegularExpression("\\d+", ErrorMessage = "Please enter a whole number.")]
        public int? NumRises { get; set; }

        [Required]
        [Display(Name = "Arc in Plan/Overall Run")]
        public decimal? OverallRun { get; set; }

        [Display(Name = "Degree Turn in Plan")]
        public decimal? DegreeTurnInPlan { get; set; }

        [Display(Name = "Tread Width & Stringer")]
        public decimal? TreadWidth { get; set; }

        [Display(Name = "Number Treads")]
        [RegularExpression("\\d+", ErrorMessage = "Please enter a whole number.")]
        public int? NumTreads { get; set; }

        [Display(Name = "Rise Per Foot")]
        public decimal? RisePerFoot { get; set; }

        [Required]
        [RegularExpression("[1-2]{1}", ErrorMessage = "Please select a Direction of Spiral from the drop-down list.")] //We want direction of spiral to be either option 1 or 2, and give the user and invalid error for selecting 0
        [Display(Name = "Direction of Spiral")]
        public string DirectionOfSpiral { get; set; }

        public IEnumerable<SelectListItem> DirectionOfSpiralList => 
            new List<SelectListItem>()
            {
                new SelectListItem("Direction of Spiral - required", "0"),
                new SelectListItem("Clockwise going up", "1"),
                new SelectListItem("Counter-Clockwise going up", "2")
            };

        [Display(Name = "Degree of Pitch (calculated)")]
        public decimal? Pitch { get; set; }

        [Display(Name = "Arc Length (calculated)")]
        public decimal? ArcLength { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}