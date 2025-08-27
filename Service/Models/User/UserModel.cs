namespace Service.Models.User
{
    /// <summary>
    /// Represents a user with basic profile, contact, and account details.
    /// Used in responses (DTO).
    /// </summary>
    public class UserModel
    {
        public long UserId { get; set; }

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Role of the user (Admin, User, Supplier).
        /// Stored as byte in DB but should ideally map to <see cref="Common.Enums.Roles"/>.
        /// </summary>
        public byte? Role { get; set; }

        public string? AvatarUrl { get; set; }

        /// <summary>
        /// User account status (true = active, false = inactive).
        /// </summary>
        public bool? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Convenience property for displaying full name.
        /// </summary>
        public string FullName => $"{Firstname} {Lastname}".Trim();
    }
}
