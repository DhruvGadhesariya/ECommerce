using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblOrder
{
    public long OrderId { get; set; }

    public long? ProductId { get; set; }

    public long? UserId { get; set; }

    public byte? Quantity { get; set; }

    public decimal? TotalAmount { get; set; }

    public byte? PaymentMethod { get; set; }

    public long? UserAddress { get; set; }

    public DateTime OrderAt { get; set; }

    public DateTime? CancledAt { get; set; }

    public virtual TblPaymentMethod? PaymentMethodNavigation { get; set; }

    public virtual TblProduct? Product { get; set; }

    public virtual TblUser? User { get; set; }

    public virtual TblUserAddress? UserAddressNavigation { get; set; }
}
