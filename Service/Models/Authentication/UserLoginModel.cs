using System.ComponentModel.DataAnnotations;

namespace Service.Models.Authentication
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please enter the Email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the Email.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
    }
}
