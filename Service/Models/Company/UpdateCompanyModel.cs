using System.ComponentModel.DataAnnotations;

namespace Service.Models.Company
{
    public class UpdateCompanyModel
    {
        [Required]
        public long CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter the Name of the Company.")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "Please Enter company location.")]
        public string CompanyAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the contactnumber of the Company.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the Email of the Company.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte? CountryId { get; set; }
    }
}
