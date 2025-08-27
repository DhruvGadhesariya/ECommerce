using System;

namespace Data.Entities
{
    /// <summary>
    /// Represents a role in the system (e.g., Admin, User, Supplier).
    /// Each role can be assigned to many users.
    /// </summary>
    public partial class UserRole
    {
        /// <summary>
        /// Primary key (e.g., 1 = Admin, 2 = User).
        /// </summary>
        public byte RoleId { get; set; }

        /// <summary>
        /// Name of the role (Admin, User, Supplier, etc.).
        /// </summary>
        public string RoleName { get; set; } = null!;

        /// <summary>
        /// Whether this role is active (soft disable option).
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// When this role was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When this role was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Soft delete timestamp (null if not deleted).
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Navigation property → users that belong to this role.
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
