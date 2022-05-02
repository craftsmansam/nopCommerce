using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.QuoteRequest
{
    public class BendingAndFabricationModel : BaseNopModel
    {
        public BendingAndFabricationModel()
        {
            SuccessfullySent = false;
        }
        [Display(Name = "Company Name")]
        [Required]
        public string CompanyName { get; set; }
        
        [Display(Name = "Primary Contact")]
        [Required]
        public string Contact { get; set; }

        [Display(Name = "Phone")]
        [Required]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "The email address submitted is invalid.")]
        public string Email { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Where did you hear about us?")]
        public string WhereHeard { get; set; }

        public IEnumerable<SelectListItem> WhereHeardList => 
            new List<SelectListItem>()
            {
                new SelectListItem("", "0"),
                new SelectListItem("ThomasNet", "1"),
                new SelectListItem("Modern Steel", "2"),
                new SelectListItem("Company Referral", "3"),
                new SelectListItem("Tradeshow", "4"),
                new SelectListItem("Mailers/Brochures", "5"),
                new SelectListItem("Trade Publications", "6"),
                new SelectListItem("Salesperson Contact (Brad Lund)", "7"),
                new SelectListItem("Social Media (Facebook, twitter, linked-in)", "8"),
                new SelectListItem("Other", "9")
            };

        [Display(Name = "Description of Project")]
        public string ProjectDescription { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "What is your bid deadline?")]
        public string BidDeadline { get; set; }

        [Display(Name = "Material Type/Size")]
        public string MaterialType { get; set; }

        [Display(Name = "Quantity")]
        public string Quantity { get; set; }
        
        [Display(Name = "Notes/Comments")]
        public string Notes { get; set; }
        public bool SuccessfullySent { get; set; }
        public bool IncludeSpiral { get; set; }
        public int UploadDocumentMaxSize { get; set; }
        
        public bool DisplayCaptcha { get; set; }
    }
}