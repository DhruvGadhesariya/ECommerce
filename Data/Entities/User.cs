using System;
using System.Collections.Generic;

namespace Data.Entities
{
    /// <summary>
    /// Represents an application user (account).
    /// Stores authentication, profile, and relationship details.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Primary key (unique user identifier).
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public string Firstname { get; set; } = null!;

        /// <summary>
        /// User's last name.
        /// </summary>
        public string Lastname { get; set; } = null!;

        /// <summary>
        /// User's email address (must be unique).
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hashed password for authentication.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// User's phone number (optional).
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Role identifier (FK to <see cref="UserRole"/>).
        /// </summary>
        public byte? Role { get; set; }

        /// <summary>
        /// Path to user avatar image (relative).
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Active/inactive status.
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// Account creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Soft delete timestamp (null if active).
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        // -------------------------
        // Navigation properties
        // -------------------------

        /// <summary>
        /// Navigation → role details.
        /// </summary>
        public virtual UserRole? RoleNavigation { get; set; }

        /// <summary>
        /// Navigation → list of addresses for this user.
        /// </summary>
        public virtual ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
    }
}
