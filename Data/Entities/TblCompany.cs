using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblCompany
{
    public long CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string CompanyAddress { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public string Email { get; set; } = null!;

    public byte? CountryId { get; set; }

    public byte? Status { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblCountry? Country { get; set; }

    public virtual TblApprovalStatus? StatusNavigation { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();

    public virtual ICollection<TblSupplier> TblSuppliers { get; set; } = new List<TblSupplier>();
}
