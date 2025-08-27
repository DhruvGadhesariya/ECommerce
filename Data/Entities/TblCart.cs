using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblCart
{
    public long CartId { get; set; }

    public long? UserId { get; set; }

    public long? ProductId { get; set; }

    public byte? Quantity { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual TblProduct? Product { get; set; }

    public virtual TblUser? User { get; set; }
}
