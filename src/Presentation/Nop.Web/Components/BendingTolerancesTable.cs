using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Calculators;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Common;
using System.Collections.Generic;

namespace Nop.Web.Components
{
    public class BendingTolerancesTableViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(BendToleranceType id, bool showFooter)
        {
            var viewModel = new BendingTolerancesTableModel
            {
                TableDescription = GetDescription(id), 
                BendToleranceRows = GetBendToleranceRows(id), 
                ShowFooter = showFooter, 
                HotOrCold = id == BendToleranceType.HotStringerLessThan20 || id == BendToleranceType.HotStringerGreaterThan20 ? "Hot" : "Cold"
            };                                                                                                                                             

            return View(viewModel);
        }

        private static List<BendingTolerancesRow> GetBendToleranceRows(BendToleranceType id)
        {
            var btRows = new List<BendingTolerancesRow>();
            switch (id)
            {
                case BendToleranceType.ColdStringerLessThan12:
                   btRows = new List<BendingTolerancesRow>
				    {
					    new("Under 10 ft", "+/- \xB9/\x2082\"", "Within 3° overall", "+/- 1°", "+/- \xB9/\x2082\"", "+/- \xB9/\x2084\""),
					    new("10 ft - 17 ft", "+/- \xB3/\x2084\"", "Within 3 \xB9/\x2082° overall", "+/- 1°", "+/- 1\"", "+/- \xB9/\x2082\""),
				    	new("17 ft - 24 ft", "+/- 1\"", "Within 4° overall", "+/- 1 \xB9/\x2082°", "+/- 1 \xB9/\x2082\"", "+/- 1\""),
				    	new("Over 24 ft", "+/- 1\"", "Within 5° overall", "+/- 1 \xB9/\x2082°", "+/- 2\"", "+/- 1\"")
				    };
                    break;
                case BendToleranceType.ColdStringerGreaterThan12:
                   btRows = new List<BendingTolerancesRow>
				    {
					    new("Under 10ft", "See Note 1", "Within 3° overall", "+/- 1°", "n/a", "n/a"),
					    new("10 ft - 17 ft", "See Note 1", "Within 3 \xB9/\x2082° overall", "+/- 1°", "n/a", "n/a"),
					    new("17 ft - 24 ft", "See Note 1", "Within 4° overall", "+/- 1 \xB9/\x2082°", "n/a", "n/a"),
					    new("Over 24 ft", "See Note 1", "Within 5° overall", "+/- 1 \xB9/\x2082°", "n/a", "n/a")
				    };
                    break;
                case BendToleranceType.HotStringerLessThan20:
                   btRows = new List<BendingTolerancesRow>
				    {
					 	new("Under 10ft", "+/- \xB9/\x2084\"", "Within 1 \xB9/\x2082° overall", "+/- \xB9/\x2082°", "+/- \xB9/\x2082\"", "+/- \xB9/\x2084\""),
					    new("10 ft - 17 ft", "+/- \xB9/\x2082\"", "Within 2° overall", "+/- \xB3/\x2084°", "+/- \xB3/\x2084\"", "+/- \xB3/\x2088°\"" ),
					    new("17 ft - 24 ft", "+/- \xB3/\x2084\"", "Within 2 \xB9/\x2082° overall", "+/- 1°", "+/- 1\"", "+/- \xB3/\x2084\""),
					    new("Over 24 ft", "+/- 1\"", "Within 3° overall", "+/- 1 \xB9/\x2082°", "+/- 1 \xB9/\x2082\"", "+/- 1\""),
				    };
                    break;
                case BendToleranceType.HotStringerGreaterThan20:
                   btRows = new List<BendingTolerancesRow>
				    {
					 	new("Under 10 ft", "See note 1", "Within 1 \xB9/\x2082° overall", "+/-  \xB9/\x2082°", "+/-  \xB9/\x2082\"", "+/-  \xB9/\x2084\""),
					    new("10 ft - 17 ft", "See note 1", "Within 2° overall", "+/- \xB3/\x2084°", "+/-  \xB3/\x2084\"", "+/-  \xB3/\x2088\""),
					    new("17 ft - 24 ft", "See note 1", "Within 2 \xB9/\x2082° overall", "+/- 1°", "+/- 1\"", "+/-  \xB3/\x2084\""),
					    new("Over 24 ft", "See note 1", "Within 3° overall", "+/- 1 \xB9/\x2082°", "+/-  1 \xB9/\x2082\"", "+/- 1\"")
				    };
                    break;
                case BendToleranceType.ColdHandrailLessThan12:
                    btRows = new List<BendingTolerancesRow>
                    {
                        new("Under 10 ft", "+/- \xB9/\x2082\"", "NA", "+/-  2 \xB9/\x2082°", "+/-  1\"", "+/-  \xB9/\x2082\""),
                        new("10 ft - 17 ft", "+/- 3/4\"", "NA", "+/- 3°", "+/-  2\"", "+/-  1\""),
                        new("17 ft - 24 ft", "+/- 1\"", "NA", "+/- 4°", "+/- 3\"", "+/-  2\""),
                    };
                    break;
                case BendToleranceType.ColdHandrailGreaterThan12:
                    btRows = new List<BendingTolerancesRow>
                    {
                        new("Under 10 ft", "See Note 1", "NA", "+/-  2 \xB9/\x2082°", "NA", "NA"),
                        new("10 ft - 17 ft", "See Note 1", "NA", "+/- 3°", "NA", "NA"),
                        new("17 ft - 24 ft", "See Note 1", "NA", "+/- 4°", "NA", "NA"),
                    };
                    break;
            }
            return btRows;
        }

        private static string GetDescription(BendToleranceType id)
        {
            var description = "";
            switch(id){
                case BendToleranceType.ColdStringerLessThan12:
                    description = "Plain view radius of 12'0\" or less.";
                    break;
                case BendToleranceType.ColdStringerGreaterThan12:
                    description = "Plain view radius greater than 12'0\"";
                    break;
                case BendToleranceType.HotStringerLessThan20:
                    description = "Plain view radius of 20'0\" or less.";
                    break;
                case BendToleranceType.HotStringerGreaterThan20:
                    description = "Plain view radius greater than 20'0\"";
                    break;
                case BendToleranceType.ColdHandrailLessThan12:
                    description = "Plain view radius of 12'0\" or less";
                    break;
                case BendToleranceType.ColdHandrailGreaterThan12:
                    description = "Plain view radius greater than 12'0\"";
                    break;
            }
            return description;
        }
    }
}
