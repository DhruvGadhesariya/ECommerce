using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.Models.Authentication
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Please enter your First name.")]
        public string Firstname { get; set; } = null!;


        [Required(ErrorMessage = "Please enter your Last name.")]
        public string Lastname { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be strong.")]
        public string Password { get; set; } =  null!;

        [Required(ErrorMessage = "Confrimpassword can't be empty.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your Phonenumber.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public byte? Role { get; set; }

        [Required]
        public byte? CountryId { get; set; }

        [Required]
        public int? CityId { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
