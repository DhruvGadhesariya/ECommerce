using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblCountry
{
    public byte CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TblCity> TblCities { get; set; } = new List<TblCity>();

    public virtual ICollection<TblCompany> TblCompanies { get; set; } = new List<TblCompany>();

    public virtual ICollection<TblUserAddress> TblUserAddresses { get; set; } = new List<TblUserAddress>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
