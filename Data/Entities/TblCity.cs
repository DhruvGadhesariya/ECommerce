using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblCity
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public byte? CountryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual TblCountry? Country { get; set; }

    public virtual ICollection<TblUserAddress> TblUserAddresses { get; set; } = new List<TblUserAddress>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
