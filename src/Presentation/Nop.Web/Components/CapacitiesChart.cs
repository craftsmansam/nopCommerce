using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class CapacitiesChartViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string chartTypeAsString = null)
        {
            CapacitiesChartType? chartType = null;
            if (chartTypeAsString != null && CapacitiesChartType.TryParse(chartTypeAsString, out CapacitiesChartType chartTypeLocal))
            {
                chartType = chartTypeLocal;
            }

            var model = new CapacitiesChartModel(chartType);

            return View(model);
        }

        public static Dictionary<CapacitiesChartType, List<CapacitiesChartRow>> CapacitiesChartDictionary =
			new Dictionary<CapacitiesChartType, List<CapacitiesChartRow>>
				{
					{
						CapacitiesChartType.AngleChartType, new List<CapacitiesChartRow>
							{
								new CapacitiesChartRow("Angle Leg Out", "/images/capchart_images/3d/anglelegout_3d_sm.png", "Angle let out 3D image",
								                       "/images/capchart_images/2d/anglelegout_2d_sm.png", "Angle leg out 2D image",
								                       "/images/anglelegout_photo_sm.png", "Angle leg out example photo",
								                       "All sizes through 8\" x 8\" x 1&frac14;\""),

								new CapacitiesChartRow("Angle Leg In", "/images/capchart_images/3d/anglelegin_3d_sm.png", "Angle leg in 3D image",
								                       "/images/capchart_images/2d/anglelegin_2d_sm.png", "Angle leg in 2D image",
								                       "/images/anglelegin_photo_sm.png",
								                       "Angle leg in example photo", "All sizes through 8\" x 8\" x 1&frac14;\""),

								new CapacitiesChartRow("Angle Heel In", "/images/capchart_images/3d/angleheelin_3d_sm.png", "Angle heel in 3D image",
								                       "/images/capchart_images/2d/angleheelin_2d_sm.png", "Angle heel in 2D image",
								                       "/images/angleheelin_photo.png",
								                       "Angle heel in photo", "All sizes through 8\" x 8\" x 1&frac14;\""),
								new CapacitiesChartRow("Angle Heel Out", "/images/capchart_images/3d/angleheelout_3d_sm.png", "Angle heel out 3D image",
								                       "/images/capchart_images/2d/angleheelout_2d_sm.png", "Angle heel out 2D image",
								                       "/images/angleheelout_photo.png", "Angle heel out photo",
								                       "All sizes through 8\" x 8\" x 1&frac14;\""),

								new CapacitiesChartRow("Angle Heel Up", "/images/capchart_images/3d/angleheelup_3d_sm.png", "Flat bar bent the hard way 3D image",
								                       "/images/capchart_images/2d/angleheelup_2d_sm.png", "Angle heel up 2D image",
								                       "/images/angleheelup_photo.png",
								                       "Angle heel up photo", "All sizes through 8\" x 8\" x 1&frac14;\""),
							}
					},
					{
						CapacitiesChartType.BarChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Flat Bar the Hard Way", "/images/capchart_images/3d/flatbarhardway_3d_sm.png", "Angle let out 3D image",
								                       "/images/capchart_images/2d/flatbarhardway_2d_sm.png", "Flat bar bent the hard way 2D image",
								                       "/images/flatbarhardway_photo.png", "Flat bar bent the hard way example photo",
								                       "Any thickness and size through 2&frac12;\" x 16\" (section is dependent on thickness to width)"),

								new CapacitiesChartRow("Plate/Flat Bar the Easy Way", "/images/capchart_images/3d/flatbareasyway_3d_sm.png", "Flat bar bent the easy way 3D image",
								                       "/images/capchart_images/2d/flatbareasyway_2d_sm.png", "Flat bar bent the easy way 2D image",
								                       "/images/flatbareasyway_photo.png", "Flat bar bent the easy way example photo",
								                       "<b>*Plate:</b>&nbsp;2-1/2\" plate up to 10'0\" in width<br/><b>*Flat Bar the Easy Way:</b>&nbsp;Any thickness and size thru 4\" x 22\"<br/>*section is dependent on thickness to width"),

								new CapacitiesChartRow("Square Bar", "/images/capchart_images/3d/squarebar_3d_sm.png", "Square bar 3D image",
								                       "/images/capchart_images/2d/squarebar_2d_sm.png", "Square bar 2D image",
								                       "/images/squarebar_photo.png", "Square bar example photo",
								                       "All Mill Produced Sizes"),
								new CapacitiesChartRow("Round Bar", "/images/capchart_images/3d/roundbar_3d_sm.png", "Round bar 3D image", "/images/capchart_images/2d/roundbar_2d_sm.png", "Round bar 2D image", "/images/roundbar_photo_sm.png", "Round bar example photo", "All Mill Produced Sizes")
								
							}
					},
					{
						CapacitiesChartType.BeamChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Beam the Easy Way (Y-Y Axis)", "/images/capchart_images/3d/beameasyway_3d_sm.png", "Beam easy way 3D image",
								                       "/images/capchart_images/2d/beameasyway_2d_sm.png", "Beam easy way 2D image",
								                       "/images/beameasyway_photo_sm.png", "Beam easy way example photo",
								                       "S3/W4 through W33 x 241#, W36 x 210# and W40 x 183#"),

								new CapacitiesChartRow("Beam the Hard Way (X-X Axis)", "/images/capchart_images/3d/beamhardway_3d_sm.png", "Beam hard way 3D image",
								                       "/images/capchart_images/2d/beamhardway_2d_sm.png", "Beam hard way 2D image",
								                       "/images/beamhardway_photo_sm.png", "Beam hard way example photo",
								                       "S3/W4 through W36 x 230#"),
							}
					},
					{
						CapacitiesChartType.ChannelChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Channel Flanges In", "/images/capchart_images/3d/channelflangesin_3d_sm.png", "Channel flanges in 3D image",
								                       "/images/capchart_images/2d/channelflangesin_2d_sm.png", "Channel flanges in 2D image",
								                       "/images/channelflangesin_photo_sm.png", "Channel flanges in example photo",
								                       "All Mill Produced Sizes"),

								new CapacitiesChartRow("Channel Flanges Out", "/images/capchart_images/3d/channelflangesout_3d_sm.png",
								                       "Channel flanges out 3D image",
								                       "/images/capchart_images/2d/channelflangesout_2d_sm.png", "Channel flanges out 2D image",
								                       "/images/channelflangesout_photo_sm.png", "Channel flanges out example photo",
								                       "All Mill Produced Sizes"),

								new CapacitiesChartRow("Channel the Hard Way (X-X Axis)", "/images/capchart_images/3d/channelhardway_3d_sm.png",
								                       "Channel hard way 3D image", "/images/capchart_images/2d/channelhardway_2d_sm.png",
								                       "Channel hard way 2D image",
								                       "/images/channelhardway_photo_sm.png", "Channel hard way example photo",
								                       "All Mill Produced Sizes"),
							}
					},
					{
						CapacitiesChartType.PlateChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Plate/Flat Bar the Easy Way", "/images/capchart_images/3d/flatbareasyway_3d_sm.png",
								                       "Flat bar easy way 3D image", "/images/capchart_images/2d/flatbareasyway_2d_sm.png",
								                       "Flat bar easy way 2D image", "/images/flatbareasyway_photo_sm.png",
								                       "Flat bar easy way example photo",
								                       "<b>*Plate:</b>&nbsp;2-1/2\" plate up to 10'0\" in width<br/><b>*Flat Bar the Easy Way:</b>&nbsp;Any thickness and size thru 4\" x 22\"<br/>*section is dependent on thickness to width")
							}
					},
					{
						CapacitiesChartType.RailChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Rail Ball In", "/images/capchart_images/3d/railballin_3d_sm.png", "Rail ball in 3D image",
								                       "/images/capchart_images/2d/railballin_2d_sm.png", "Rail ball in 2D image", "/images/railballin_photo.png",
								                       "Rail ball in photo", "All sizes up to approximately 175#"),

								new CapacitiesChartRow("Rail Ball Out", "/images/capchart_images/3d/railballout_3d_sm.png", "Rail ball out 3D image",
								                       "/images/capchart_images/2d/railballout_2d_sm.png", "Rail ball out 2D image",
								                       "/images/railballout_photo_sm.png", "Rail ball out example photo",
								                       "All sizes up to approximately 175#"),

								new CapacitiesChartRow("Rail Ball Up", "/images/capchart_images/3d/railballup_3d_sm.png", "Rail ball up 3D image",
								                       "/images/capchart_images/2d/railballup_2d_sm.png", "Rail ball up 2D image",
								                       "/images/railballup_photo_sm.png",
								                       "Rail ball up example photo", "All sizes up to approximately 175#"),
							}
					},
					{
						CapacitiesChartType.RoundPipeTubeChartType, new List<CapacitiesChartRow>
							{
								new CapacitiesChartRow("Round Tube &amp; Pipe", "/images/capchart_images/3d/roundtube_3d_sm.png", "Round tube 3D image",								                       "/images/capchart_images/2d/roundtube_2d_sm.png", "Round tube 2D image", "/images/roundtube_photo_sm.png",
								                       "Round tube example photo",
                                      "<b>Roll/Cold Bending:</b> 3&frasl;16\" OD through 28\" OD <br /> <b>Induction/Hot Bending:</b> 6\" OD through 20\" OD <br /><b>Rotary Draw/Mandrel Bending:</b> &frac12;\" OD through 6\" pipe (Sch80)")
							}
					},
					{
						CapacitiesChartType.SquareRectangularTubeChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Rectangular Tube the Hard Way", "/images/capchart_images/3d/recttubehard_3d_sm.png",
								                       "Rectangular tube hard way 3D image", "/images/capchart_images/2d/recttubehard_2d_sm.png",
								                       "Rectangular tube hard way 2D image", "/images/recttubehard_photo_sm.png",
								                       "Rectangular tube hard way example photo",
								                       "20\" x 12\" x .625\" (Maximum mill produced size-bending capacity is greater)"),

								new CapacitiesChartRow("Rectangular Tube the Easy Way", "/images/capchart_images/3d/rectangletubeeasyway_3d_sm.png",
								                       "Rectangular tube easy way 3D image", "/images/capchart_images/2d/rectangletubeeasyway_2d_sm.png",
								                       "Rectangular tube easy way 2D image", "/images/recttubeeasy_photo_sm.png",
								                       "Rectangular tube easy way example photo",
								                       "20\" x 12\" x .625\" (Maximum mill produced size-bending capacity is greater)"),

								new CapacitiesChartRow("Square Tube", "/images/capchart_images/3d/squaretube_3d_sm.png", "Square tube 3D image",
								                       "/images/capchart_images/2d/squaretube_2d_sm.png", "Square tube 2D image", "/images/squaretube_photo_sm.png",
								                       "Square tube example photo",
								                       "16\" x 16\" x .625\" (Maximum mill produced size-bending capacity is greater)"),

								new CapacitiesChartRow("Square Tube Diagonally", "/images/capchart_images/3d/squaretubediag_3d_sm.png",
								                       "Square tube diagonally 3D image", "/images/capchart_images/2d/squaretubediag_2d_sm.png",
								                       "Square tube diagonally 2D image", "/images/squaretubediag_photo_sm.png",
								                       "Square tube diagonally example photo",
								                       "16\" x 16\" x .625\" (Maximum mill produced size-bending capacity is greater)")
							}
					},
					{
						CapacitiesChartType.TeeChartType, new List<CapacitiesChartRow>
							{

								new CapacitiesChartRow("Tee Stem In", "/images/capchart_images/3d/teestemin_3d_sm.png", "Tee stem in 3D image",
								                       "/images/capchart_images/2d/teestemin_2d_sm.png", "Tee stem in 2D image", "/images/teestemin_photo_sm.png",
								                       "Tee stem in example photo",
								                       "All sizes through 12\" Stems (weight per foot maximums would need to be calculated based on WF origin)"),

								new CapacitiesChartRow("Tee Stem Out", "/images/capchart_images/3d/teestemout_3d_sm.png", "Tee stem out 3D image",
								                       "/images/capchart_images/2d/teestemout_2d_sm.png", "Tee stem out 2D image",
								                       "/images/teestemout_photo_sm.png",
								                       "Tee stem out example photo",
								                       "All sizes through 12\" Stems (weight per foot maximums would need to be calculated based on WF origin)"),

								new CapacitiesChartRow("Tee Stem Up", "/images/capchart_images/3d/teestemup_3d_sm.png", "Tee stem up 3D image",
								                       "/images/capchart_images/2d/teestemup_2d_sm.png", "Tee stem up 2D image", "/images/teestemup_photo_sm.png",
								                       "Tee stem up example photo",
								                       "All sizes through 12\" Stems (weight per foot maximums would need to be calculated based on WF origin)"),
							}
					},
					{
						CapacitiesChartType.BulbFlatChartType, new List<CapacitiesChartRow>
							{
								new CapacitiesChartRow("Bulb Flat", "/images/capchart_images/3d/bulbflat_3d_sm.png", "Bulb flat in 3D image",
								                       "/images/capchart_images/2d/bulbflat_2d_sm.png", "Bulb flat in 2D image", "/images/bulbflat_photo_sm.png",
								                       "Bulb flat in example photo",
								                       "All mill sized produced up to 430 mm x 20 mm (16.93\" wide x .787\" thick)."),
							}
					},
				};
    }

    public enum CapacitiesChartType
    {
        AngleChartType,
        BarChartType,
        BeamChartType,
        ChannelChartType,
        PlateChartType,
        RailChartType,
        RoundPipeTubeChartType,
        SquareRectangularTubeChartType,
        TeeChartType,
        BulbFlatChartType
    }

    public class CapacitiesChartModel
    {
		public CapacitiesChartType? ChartType { get; }

        public IEnumerable<CapacitiesChartType> ListOfChartTypes
        {
            get
            {
                if (ChartType.HasValue)
                {
                    return new List<CapacitiesChartType> {ChartType.Value};
                }

                return Enum.GetValues(typeof(CapacitiesChartType)).Cast<CapacitiesChartType>().ToList();
            }
        }

        public string CapChartHeaderHtml
        {
            get
            {
                var hWhat = 1;
                if (ChartType.HasValue)
                {
                    hWhat = 2;
                }

                var addBreak = (ChartType.HasValue ? string.Empty : "<br/>");
                return $"<h{hWhat} class=\"capchartheader\">Capacities Chart</h{hWhat}>{addBreak}";
            }
        }

        public CapacitiesChartModel(CapacitiesChartType? chartType)
        {
            ChartType = chartType;
        }
    }

    public class CapacitiesChartRow
    {
        public string Section { get; }
        public string ThreeDImg { get; }
        public string ThreeDAltTitle { get; }
        public string TwoDImg { get; }
        public string TwoDAltTitle { get; }
        public string PhotoImg { get; }
        public string PhotoAltTitle { get; }
        public string Capacities { get; }

        public CapacitiesChartRow(string section, string threeDImg, string threeDAltTitle, string twoDImg, string twoDAltTitle, string photoImg, string photoAltTitle, string capacities)
        {
            Section = section;
            ThreeDImg = threeDImg;
            ThreeDAltTitle = threeDAltTitle;
            TwoDImg = twoDImg;
            TwoDAltTitle = twoDAltTitle;
            PhotoImg = photoImg;
            PhotoAltTitle = photoAltTitle;
            Capacities = capacities;
        }
    }

}
