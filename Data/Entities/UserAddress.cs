using System;
using System.Collections.Generic;

namespace Data.Entities
{
    /// <summary>
    /// Stores address information for a user.
    /// Each user may have one or more addresses.
    /// </summary>
    public partial class UserAddress
    {
        /// <summary>
        /// Primary key for the address.
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// First line of the address (street, building, etc.).
        /// </summary>
        public string AddressLine1 { get; set; } = null!;

        /// <summary>
        /// Second line of the address (optional).
        /// </summary>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// Foreign key → User who owns this address.
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Timestamp when address was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when address was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Soft delete timestamp (null if active).
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Navigation property → user who owns this address.
        /// </summary>
        public virtual User? User { get; set; }
    }
}
