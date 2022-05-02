using System.IO;
using Albina.SpiralMath;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Services.Calculators;
using Nop.Web.Models.Calculators;
using System.Linq;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;

namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CalculatorsController : Controller
    {
        private readonly ITangentMaterialService _tangentMaterialService;

        protected readonly AlbinaConfig AlbinaWebConfig;
        protected readonly IWorkContext WorkContext;

        public CalculatorsController(IWorkContext context, ITangentMaterialService tangentMaterialService)
        {
            AlbinaWebConfig = EngineContext.Current.Resolve<AlbinaConfig>();
            WorkContext = context;
            _tangentMaterialService = tangentMaterialService;
        }


        public IActionResult BendingCalculators()
        {
            return View();
        }

        public IActionResult SpiralCalculator()
        {
            var viewModel = new SpiralCalculatorModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SpiralCalculator(SpiralCalculatorModel spiralCalculatorModel)
        {
            if (ModelState.IsValid)
            {
                CalculateAndBind(ref spiralCalculatorModel);
                return Json(spiralCalculatorModel);
            }
            return Json(new {Result = "Error" });
        }

        [HttpPost]
        public IActionResult PrintSpiralReport(SpiralCalculatorModel spiralCalculatorModel)
        {
            if (ModelState.IsValid)
            {
                var ds  = CalculateAndBind(ref spiralCalculatorModel);

                var filename = SpiralMathReportHelper.SpiralMathReportCacheFileName(AlbinaWebConfig.SpiralMathReportCache, WorkContext.CurrentCustomer.CustomerGuid.ToString());
                var radiusInInches = ds.SpiralMath[0].IsActualRadiusNull() ? 0 : ds.SpiralMath[0].ActualRadius;
                SpiralMathReportHelper.InitializeSpiralMathData(ds, radiusInInches, false, Path.GetFileName(filename));

                CreateReport(ds, filename);
                var filenameOnly = Path.GetFileName(filename);

                return Json(filenameOnly);
            }
            return Json(new {Result = "Error" });
        }

        public IActionResult DownloadSpiralReport()
        {
            var filename = SpiralMathReportHelper.SpiralMathReportCacheFileName(AlbinaWebConfig.SpiralMathReportCache, WorkContext.CurrentCustomer.CustomerGuid.ToString());
            var fs = System.IO.File.OpenRead(filename);
            return File(fs, System.Net.Mime.MediaTypeNames.Application.Pdf, "SpiralReport.pdf");
        }

        private static void CreateReport(SpiralMathData ds, string filename)
        {
            var report = new SpiralMathReport()
            {
                DataSource = ds,
                DataMember = ds.Tables[0].TableName
            };
            report.ExportToPdf(filename);
        }

        private static SpiralMathData CalculateAndBind(ref SpiralCalculatorModel spiralCalculatorModel)
        {
            var ds = new SpiralMathData();
            var row = ds.SpiralMath.NewSpiralMathRow();

            ParseModel(row, spiralCalculatorModel);

            ds.SpiralMath.AddSpiralMathRow(row);

            var calculator = new Calculator(row);
            calculator.CalculateFields();

            spiralCalculatorModel = BindModel(row);

            return ds;
        }

        private static SpiralCalculatorModel BindModel(SpiralMathData.SpiralMathRow row)
        {
            var spiralCalculatorModel = new SpiralCalculatorModel
            {
                PlanViewRadius = row.PlanViewRadius,
                PlanViewDiameter = row.IsPlanViewDiameterNull() ? (decimal?)null : row.PlanViewDiameter,
                OverallHeight = row.IsOverallHeightNull() ? (decimal?)null : row.OverallHeight,
                RiseHeight = row.IsRiseHeightNull() ? null : (decimal?) row.RiseHeight,
                NumRises = row.IsNumRisesNull() ? null : (int?) row.NumRises,
                NumTreads = row.IsNumTreadsNull() ? null : (int?) row.NumTreads,
                OverallRun = row.OverallRun,
                DegreeTurnInPlan = row.IsDegreeTurnInPlanNull() ? null : (decimal?) row.DegreeTurnInPlan,
                TreadWidth = row.IsTreadWidthNull() ? null : (decimal?) row.TreadWidth,
                Pitch = row.IsPitchNull() ? null : (decimal?) row.Pitch,
                ArcLength = row.IsArcLengthNull() ? null : (decimal?) row.ArcLength,
                RisePerFoot = row.IsRisePerFootNull() ? null : (decimal?) row.RisePerFoot
            };

            spiralCalculatorModel.DirectionOfSpiral = row.IsDirectionOfSpiralNull() ? null : spiralCalculatorModel.DirectionOfSpiralList.SingleOrDefault(x => x.Text == row.DirectionOfSpiral)?.Value;
            spiralCalculatorModel.Notes = row.IsNotesNull() ? null : row.Notes;

            return spiralCalculatorModel;
        }

        private static void ParseModel(SpiralMathData.SpiralMathRow row, SpiralCalculatorModel spiralCalculatorModel)
        {
            row.PlanViewRadius = spiralCalculatorModel.PlanViewRadius ?? 0;
            if (spiralCalculatorModel.PlanViewDiameter != null)
            {
                row.PlanViewDiameter = spiralCalculatorModel.PlanViewDiameter.Value;
            }
            row.OverallHeight = spiralCalculatorModel.OverallHeight ?? 0;
            if (spiralCalculatorModel.RiseHeight != null)
            {
                row.RiseHeight = spiralCalculatorModel.RiseHeight.Value;
            }

            if (spiralCalculatorModel.NumRises != null)
            {
                row.NumRises = spiralCalculatorModel.NumRises.Value;
            }

            if (spiralCalculatorModel.NumTreads != null)
            {
                row.NumTreads = spiralCalculatorModel.NumTreads.Value;
            }

            row.OverallRun = spiralCalculatorModel.OverallRun ?? 0;
            if (spiralCalculatorModel.DegreeTurnInPlan != null)
            {
                row.DegreeTurnInPlan = spiralCalculatorModel.DegreeTurnInPlan.Value;
            }

            if (spiralCalculatorModel.TreadWidth != null)
            {
                row.TreadWidth = spiralCalculatorModel.TreadWidth.Value;
            }

            if (spiralCalculatorModel.Pitch != null)
            {
                row.Pitch = spiralCalculatorModel.Pitch.Value;
            }

            if (spiralCalculatorModel.ArcLength != null)
            {
                row.ArcLength = spiralCalculatorModel.ArcLength.Value;
            }

            if (spiralCalculatorModel.RisePerFoot != null)
            {
                row.RisePerFoot = spiralCalculatorModel.RisePerFoot.Value;
            }
            if(spiralCalculatorModel.DirectionOfSpiral != null)
            {
                row.DirectionOfSpiral = spiralCalculatorModel.DirectionOfSpiralList.SingleOrDefault(x => x.Value == spiralCalculatorModel.DirectionOfSpiral)?.Text;
            }

            row.Notes = spiralCalculatorModel.Notes;
        }

        public IActionResult TangentMaterials()
        {
            var viewModel = new TangentMaterialsModel();
            var materials = _tangentMaterialService.GetAllUniqueMaterialNames();
            foreach(var material in materials)
            {
                viewModel.MaterialSelectorList.Add(new SelectListItem(material, material));
            }

            var materialSizes = _tangentMaterialService.GetAllMaterialSizes();
            foreach(var materialSize in materialSizes)
            {
                viewModel.MaterialSizeSelectorList.Add(new SelectListItem(materialSize, materialSize));
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult TangentMaterialsCalculate(string size, string material)
        {
            if (ModelState.IsValid)
            {
                var drawBendResult = _tangentMaterialService.GetTangentMaterialResult("Rotary Draw", material, size);
                var rollBendResult = _tangentMaterialService.GetTangentMaterialResult("Roll Bend", material, size);
                return Json(new {DrawBendResult = drawBendResult, RollBendResult = rollBendResult});
            }

            return Json(new {Result = "Error" });
        }

        public IActionResult BendingTolerances()
        {
            return View();
        }
    }
}
