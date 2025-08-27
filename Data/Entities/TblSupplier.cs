using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblSupplier
{
    public short SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? ContactNumber { get; set; }

    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }

    public byte? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CompanyId { get; set; }

    public virtual TblCompany? Company { get; set; }

    public virtual TblApprovalStatus? StatusNavigation { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
}
