using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblPaymentMethod
{
    public byte MethodId { get; set; }

    public string MethodName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}
