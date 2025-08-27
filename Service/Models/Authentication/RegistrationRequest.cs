using System.ComponentModel.DataAnnotations;

namespace Service.Models.Authentication
{
    /// <summary>
    /// DTO for user registration request.
    /// Includes validation attributes for required fields.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// User's first name (required, max 16 characters).
        /// </summary>
        [Required, StringLength(16)]
        public string Firstname { get; set; } = string.Empty;

        /// <summary>
        /// User's last name (required, max 16 characters).
        /// </summary>
        [Required, StringLength(16)]
        public string Lastname { get; set; } = string.Empty;

        /// <summary>
        /// User's email (required, valid email format, max 255 characters).
        /// </summary>
        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password (required, min 6 characters).
        /// ⚠️ Must be securely hashed before storing in the database.
        /// </summary>
        [Required, StringLength(255, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
