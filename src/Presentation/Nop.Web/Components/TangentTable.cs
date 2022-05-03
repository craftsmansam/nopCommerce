using Microsoft.AspNetCore.Mvc;
using Nop.Services.Calculators;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Components
{
    public class TangentTableViewComponent : NopViewComponent
    {
        private readonly ITangentMaterialService _tangentMaterialService;
        public TangentTableViewComponent(ITangentMaterialService tangentMaterialService)
        {
          _tangentMaterialService = tangentMaterialService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string bendType, string headerTableID, string mainTableID, string classPrefix)
        {
            var viewModel = new TangentTableModel();
            viewModel.BendType = bendType;
            viewModel.HeaderTableID = headerTableID;
            viewModel.MainTableID = mainTableID;
            viewModel.ClassPrefix = classPrefix;
            viewModel.TangentMaterials = (await _tangentMaterialService.GetTangentTableAsync()).Where(x => x.BendType == bendType);

            return View(viewModel);
        }
    }
}
