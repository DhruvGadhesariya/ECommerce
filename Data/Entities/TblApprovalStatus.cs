using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblApprovalStatus
{
    public byte StatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<TblCompany> TblCompanies { get; set; } = new List<TblCompany>();

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();

    public virtual ICollection<TblSupplier> TblSuppliers { get; set; } = new List<TblSupplier>();
}
