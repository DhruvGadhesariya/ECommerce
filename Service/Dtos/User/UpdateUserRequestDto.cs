using System.ComponentModel.DataAnnotations;

namespace Service.Dtos.User
{
    /// <summary>
    /// DTO used for updating an existing user's details.
    /// Validated via data annotations before processing in the controller.
    /// </summary>
    public class UpdateUserRequestDto
    {
        /// <summary>
        /// Unique identifier of the user to update.
        /// </summary>
        [Required]
        public long UserId { get; set; }

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
        /// User's email address (max 255 characters, must be valid format).
        /// </summary>
        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;

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

        /// <summary>
        /// User status (true = active, false = inactive).
        /// </summary>
        public bool? Status { get; set; }
    }
}
