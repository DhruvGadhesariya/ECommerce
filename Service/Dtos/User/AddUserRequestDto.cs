using System.ComponentModel.DataAnnotations;

namespace Service.Dtos.User
{
    /// <summary>
    /// DTO used for creating a new user.
    /// Includes validation rules enforced via data annotations.
    /// </summary>
    public class AddUserRequestDto
    {
        /// <summary>
        /// User's first name (max 16 characters).
        /// </summary>
        [Required, StringLength(16)]
        public string Firstname { get; set; } = string.Empty;

        /// <summary>
        /// User's last name (max 16 characters).
        /// </summary>
        [Required, StringLength(16)]
        public string Lastname { get; set; } = string.Empty;

        /// <summary>
        /// User's email address (must be valid format, max 255 characters).
        /// </summary>
        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password (required, min 6 characters).
        /// Should be securely hashed before storage.
        /// </summary>
        [Required, StringLength(255, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// User's phone number (optional, validated by PhoneAttribute).
        /// </summary>
        [Phone]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// User role (Admin, User, Supplier).
        /// Stored as byte but should ideally map to <see cref="Common.Enums.Roles"/>.
        /// </summary>
        public byte? Role { get; set; }
    }
}
