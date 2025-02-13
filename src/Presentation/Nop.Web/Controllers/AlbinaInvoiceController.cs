using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Craftsman.DesignByContract;
using Craftsman.IO;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.AlbinaInvoice;
using Nop.Services.Security;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.AlbinaInvoice;

namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AlbinaInvoiceController : BasePublicController
    {
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IAlbinaInvoiceService _invoiceService;

        public AlbinaInvoiceController(IPermissionService permissionService, IWorkContext workContext, IAlbinaInvoiceService invoiceService)
        {
            _permissionService = permissionService;
            _workContext = workContext;
            _invoiceService = invoiceService;
        }

        [CheckPermission(StandardPermission.PublicStore.ALBINA_INVOICE)]
        public async Task<IActionResult> InvoiceListAsync()
        {

            var customer = await _workContext.GetCurrentCustomerAsync();
            var itemsTable = await _invoiceService.CustomerListInvoicesAsync(customer.SalesContactID);

            var showInvoiceUrl = $"/secure/show-invoice?cscid={customer.SalesContactID}&id=";
            var models = itemsTable.Select(x =>
            {
                var url = $"{showInvoiceUrl}{x.ShopOrderContentCentralInvoiceID}";

                return new AlbinaInvoiceModel(x.ShopOrderNumber, x.InvoiceNumber, x.InvoiceDate, url);
            });
            
            var model = new List<AlbinaInvoiceModel>(models);

            return View(model);
        }

        [CheckPermission(StandardPermission.PublicStore.ALBINA_INVOICE)]
        public async Task<IActionResult> ShowInvoiceAsync(string cscid, string id)
        {

            var customer = await _workContext.GetCurrentCustomerAsync();

            Check.RequireNotNull(customer.SalesContactID);

            if (id != null && int.TryParse(id, out var shopOrderContentCentralId))
            {
                if (cscid != null && int.TryParse(cscid, out var customerSalesContactId))
                {
                    var isInvoiceForMyCompany = await _invoiceService.IsInvoiceForMyCompanyAsync(customerSalesContactId, shopOrderContentCentralId);

                    Check.Require(isInvoiceForMyCompany, "Someone is potentially trying to access some else's invoices!");
                    Check.Require(customer.SalesContactID == customerSalesContactId, "The logged in user is potentially trying to look at someone else's invoices!");

                    var fullFilePath = await _invoiceService.InvoiceFileNameByShopOrderContentCentralInvoiceIdAsync(shopOrderContentCentralId);
                    var fileExtension = Path.GetExtension(fullFilePath).ToLower().Replace(".", "");

                    Check.Require(System.IO.File.Exists(fullFilePath), $"The file {fullFilePath} doesn't exist!");

                    var mimeType = MimeType.All.First(x => x.FileExtensions.Any(y => y.Extension.ToLower() == fileExtension));

                    var fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);
                    
                    return File(fileStream, mimeType.MimeTypeString);
                }
            }

            throw new ArgumentException($"{nameof(ShowInvoiceAsync)} was called with invalid arguments!");
        }
    }
}
