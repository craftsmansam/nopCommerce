using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.QuoteRequest
{
    public record BendingAndFabricationModel : BaseNopModel
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