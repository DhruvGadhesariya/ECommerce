using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblUserAddress
{
    public long AddressId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public int PinCode { get; set; }

    public long? UserId { get; set; }

    public byte? CountryId { get; set; }

    public int? CityId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblCity? City { get; set; }

    public virtual TblCountry? Country { get; set; }

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual TblUser? User { get; set; }
}
