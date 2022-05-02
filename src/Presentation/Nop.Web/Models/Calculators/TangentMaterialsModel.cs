using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Web.Models.Calculators
{
    public class TangentMaterialsModel
    {
        public TangentMaterialsModel()
        {
            MaterialSelectorList = new List<SelectListItem>();
            MaterialSizeSelectorList = new List<SelectListItem>();
        }

        public List<SelectListItem> MaterialSelectorList {get; set;}

        public string MaterialSelector {get; set;}

        public List<SelectListItem> MaterialSizeSelectorList {get; set;}

        public string MaterialSizeSelector {get; set;}
    }
}