using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblUser
{
    public long UserId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public byte? Role { get; set; }

    public string? Avatar { get; set; }

    public bool? Status { get; set; }

    public byte? CountryId { get; set; }

    public int? CityId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblCity? City { get; set; }

    public virtual TblCountry? Country { get; set; }

    public virtual TblUserRole? RoleNavigation { get; set; }

    public virtual ICollection<TblCart> TblCarts { get; set; } = new List<TblCart>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual ICollection<TblProductRating> TblProductRatings { get; set; } = new List<TblProductRating>();

    public virtual ICollection<TblProductReview> TblProductReviews { get; set; } = new List<TblProductReview>();

    public virtual ICollection<TblUserAddress> TblUserAddresses { get; set; } = new List<TblUserAddress>();
}
