using System.ComponentModel.DataAnnotations;

namespace Service.Models.Authentication
{
    /// <summary>
    /// DTO for user login request.
    /// Requires email and password fields.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User's email (required, must be a valid email address).
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password (required).
        /// ⚠️ Must be securely compared against a hashed password in the database.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
