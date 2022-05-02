using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Calculators;
using Nop.Services.Calculators;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Common;
using System.Collections.Generic;

namespace Nop.Web.Components
{
    public class BendingTolerancesTableViewComponent : NopViewComponent
    {
        private readonly ITangentMaterialService _tangentMaterialService;
        public BendingTolerancesTableViewComponent(ITangentMaterialService tangentMaterialService)
        {
          _tangentMaterialService = tangentMaterialService;
        }

        public IViewComponentResult Invoke(int id, bool showFooter, string hotOrCold)
        {
            var viewModel = new BendingTolerancesTableModel();
            viewModel.TableDescription = GetDescription(id);
            viewModel.BTRows = GetBTRows(id);
            viewModel.ShowFooter = showFooter;
            viewModel.HotOrCold = hotOrCold;
            return View(viewModel);
        }

        private List<BendingTolerancesRow> GetBTRows(int id)
        {
            var BTRows = new List<BendingTolerancesRow>();
            switch (id)
            {
                case 1:
                   BTRows = new List<BendingTolerancesRow>
				    {
					    new BendingTolerancesRow("Under 10 ft", "+/- \xB9/\x2082\"", "Within 3° overall", "+/- 1°", "+/- \xB9/\x2082\"", "+/- \xB9/\x2084\""),
					    new BendingTolerancesRow("10 ft - 17 ft", "+/- \xB3/\x2084\"", "Within 3 \xB9/\x2082° overall", "+/- 1°", "+/- 1\"", "+/- \xB9/\x2082\""),
				    	new BendingTolerancesRow("17 ft - 24 ft", "+/- 1\"", "Within 4° overall", "+/- 1 \xB9/\x2082°", "+/- 1 \xB9/\x2082\"", "+/- 1\""),
				    	new BendingTolerancesRow("Over 24 ft", "+/- 1\"", "Within 5° overall", "+/- 1 \xB9/\x2082°", "+/- 2\"", "+/- 1\"")
				    };
                    break;
                case 2:
                   BTRows = new List<BendingTolerancesRow>
				    {
					    new BendingTolerancesRow("Under 10ft", "See Note 1", "Within 3° overall", "+/- 1°", "n/a", "n/a"),
					    new BendingTolerancesRow("10 ft - 17 ft", "See Note 1", "Within 3 \xB9/\x2082° overall", "+/- 1°", "n/a", "n/a"),
					    new BendingTolerancesRow("17 ft - 24 ft", "See Note 1", "Within 4° overall", "+/- 1 \xB9/\x2082°", "n/a", "n/a"),
					    new BendingTolerancesRow("Over 24 ft", "See Note 1", "Within 5° overall", "+/- 1 \xB9/\x2082°", "n/a", "n/a")
				    };
                    break;
                case 3:
                   BTRows = new List<BendingTolerancesRow>
				    {
					 	new BendingTolerancesRow("Under 10ft", "+/- \xB9/\x2084\"", "Within 1 \xB9/\x2082° overall", "+/- \xB9/\x2082°", "+/- \xB9/\x2082\"", "+/- \xB9/\x2084\""),
					    new BendingTolerancesRow("10 ft - 17 ft", "+/- \xB9/\x2082\"", "Within 2° overall", "+/- \xB3/\x2084°", "+/- \xB3/\x2084\"", "+/- \xB3/\x2088°\"" ),
					    new BendingTolerancesRow("17 ft - 24 ft", "+/- \xB3/\x2084\"", "Within 2 \xB9/\x2082° overall", "+/- 1°", "+/- 1\"", "+/- \xB3/\x2084\""),
					    new BendingTolerancesRow("Over 24 ft", "+/- 1\"", "Within 3° overall", "+/- 1 \xB9/\x2082°", "+/- 1 \xB9/\x2082\"", "+/- 1\""),
				    };
                    break;
                case 4:
                   BTRows = new List<BendingTolerancesRow>
				    {
					 	new BendingTolerancesRow("Under 10 ft", "See note 1", "Within 1 \xB9/\x2082° overall", "+/-  \xB9/\x2082°", "+/-  \xB9/\x2082\"", "+/-  \xB9/\x2084\""),
					    new BendingTolerancesRow("10 ft - 17 ft", "See note 1", "Within 2° overall", "+/- \xB3/\x2084°", "+/-  \xB3/\x2084\"", "+/-  \xB3/\x2088\""),
					    new BendingTolerancesRow("17 ft - 24 ft", "See note 1", "Within 2 \xB9/\x2082° overall", "+/- 1°", "+/- 1\"", "+/-  \xB3/\x2084\""),
					    new BendingTolerancesRow("Over 24 ft", "See note 1", "Within 3° overall", "+/- 1 \xB9/\x2082°", "+/-  1 \xB9/\x2082\"", "+/- 1\"")
				    };
                    break;
            }
            return BTRows;
        }

        private string GetDescription(int id)
        {
            var description = "";
            switch(id){
                case 1:
                    description = "Plain view radius of 12'0\" or less.";
                    break;
                case 2:
                    description = "Plain view radius greater than 12'0\"";
                    break;
                case 3:
                    description = "Plain view radius of 20'0\" or less.";
                    break;
                case 4:
                    description = "Plain view radius greater than 20'0\"";
                    break;
            }
            return description;
        }
    }
}
