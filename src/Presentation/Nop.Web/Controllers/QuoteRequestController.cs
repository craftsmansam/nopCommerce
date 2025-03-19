using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Albina.SpiralMath;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Domain.Security;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.QuoteRequest;

namespace Nop.Web.Controllers
{
    public class QuoteRequestController : BasePublicController
    {
        private readonly CaptchaSettings _captchaSettings;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly AlbinaConfig _albinaConfig;
        private readonly IHtmlFormatter _htmlFormatter;

        public QuoteRequestController(CaptchaSettings captchaSettings,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            IHostEnvironment hostEnvironment,
            AlbinaConfig albinaConfig,
            IHtmlFormatter htmlFormatter)
        {
            _htmlFormatter = htmlFormatter;
            _captchaSettings = captchaSettings;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _stateProvinceService = stateProvinceService;
            _workContext = workContext;
            _workflowMessageService = workflowMessageService;
            _hostEnvironment = hostEnvironment;
            _albinaConfig = albinaConfig;
        }

        public async Task<IActionResult> BendingAndFabrication()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var model = new BendingAndFabricationModel
            {
                IncludeSpiral = HttpContext.Request.Query["include_spiral"].Equals("true"),
                UploadDocumentMaxSize = _albinaConfig.UploadDocumentMaxSize,
                Email = customer.Email,
                Contact = await _customerService.GetCustomerFullNameAsync(customer),
                CompanyName = customer.Company,
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage
            };

            var firstAddress = (await _customerService.GetAddressesByCustomerIdAsync(customer.Id)).OrderByDescending(x => x.CreatedOnUtc).FirstOrDefault();
            if (firstAddress != null)
            {
                if (firstAddress.Company != null)
                {
                    model.CompanyName = firstAddress.Company;
                }
                model.Phone = firstAddress.PhoneNumber;
                var address = firstAddress.Address1 ?? firstAddress.Address2;
                if (firstAddress.Address1 != null)
                {
                    address += firstAddress.Address2 != null ? " " + firstAddress.Address2 : "";
                }
                model.Address = address;
                model.City = firstAddress.City;
                if (firstAddress.StateProvinceId != null)
                {
                    model.State = (await _stateProvinceService.GetStateProvinceByIdAsync(firstAddress.StateProvinceId.Value)).Name;
                }
                model.ZipCode = firstAddress.ZipPostalCode;
            }

            return View(model);
        }

        [HttpPost, ActionName("BendingAndFabrication")]
        [ValidateCaptcha]
        public virtual async Task<IActionResult> BendingAndFabricationSend(BendingAndFabricationModel model, bool captchaValid, List<IFormFile> files)
        {
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
            {
                ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
            }

            if (ModelState.IsValid)
            {
                var subject = "Bending and Fabrication Quote Request from " + model.CompanyName;
                var body = "Company Name: " + model.CompanyName + "\n\nContact Name: " + model.Contact + "\n\nPhone: " + model.Phone + 
                    "\n\nEmail: " + model.Email + "\n\nFax: " + model.Fax + "\n\nAddress: " + model.Address + "\n\nCity: " + model.City  +
                    "\n\nState: " + model.State + "\n\nZip: " + model.ZipCode + "\n\nWhere did you hear about us: " + 
                    model.WhereHeardList.Single(x => x.Value == model.WhereHeard).Text + "\n\nProject Description:\n" +
                    model.ProjectDescription + "\n\nProject Name: " + model.ProjectName + "\n\nBid Deadline: " + model.BidDeadline +
                    "\n\nMaterial Size/Type: " + model.MaterialType + "\n\nQuantity: " + model.Quantity + "\n\nNotes: " + model.Notes;
                var bodyFormatted = _htmlFormatter.FormatText(body, false, true, false, false, false, false);
                var attachments = new List<string>();

                var customer = await _workContext.GetCurrentCustomerAsync();
                var language = await _workContext.GetWorkingLanguageAsync();

                foreach (var formFile in files)
                {
                    var fileName = SaveFileToServer(formFile.OpenReadStream(), formFile.FileName);
                    attachments.Add(fileName);
                }
                
                if (model.IncludeSpiral)
                {
                    var spiralFileName = SpiralMathReportHelper.SpiralMathReportCacheFileName(_albinaConfig.SpiralMathReportCache, customer.CustomerGuid.ToString());
                    //copy File out to Server so that we don't delete the original with email send and they can download the original after email send
                    var newFileName = SaveFileToServer(System.IO.File.OpenRead(spiralFileName), model.CompanyName + "SpiralReport.pdf");
                    attachments.Add(newFileName);
                }

                await _workflowMessageService.SendQuoteEmailAsync(language.Id, model.Email.Trim(), model.Contact, subject, bodyFormatted, attachments);

                model.SuccessfullySent = true;
            }

            if (model.SuccessfullySent)
            {
                return RedirectToAction("BendingAndFabricationGoogle");
            }

            return View(model);
        }

        public async Task<IActionResult> BendingAndFabricationSuccess()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var model = new BendingAndFabricationSuccessModel
            {
                IncludeSpiral = System.IO.File.Exists(SpiralMathReportHelper.SpiralMathReportCacheFileName(_albinaConfig.SpiralMathReportCache, customer.CustomerGuid.ToString()))
            };
            return View(model);
        }

        public IActionResult BendingAndFabricationGoogle()
        {
            return View();
        }

        private string SaveFileToServer(Stream fileStream, string filename)
        {
            var contentRootPath = _hostEnvironment.ContentRootPath;
            var directoryPath = Path.Combine(contentRootPath, "email_uploads", "rfq");
            Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, filename);
            while (System.IO.File.Exists(filePath))
            {
                var filenameNoExtenstion = Path.GetFileNameWithoutExtension(filePath);
                var extension = Path.GetExtension(filePath);
                filePath = Path.Combine(directoryPath, filenameNoExtenstion + "(1)" + extension);
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            fileStream.CopyTo(stream);
            return filePath;
        }
    }
}
