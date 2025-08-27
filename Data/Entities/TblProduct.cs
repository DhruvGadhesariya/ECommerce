using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblProduct
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string StokeKeepingUnit { get; set; } = null!;

    public bool? IsFree { get; set; }

    public byte? Status { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public short? SupplierId { get; set; }

    public long? CompanyId { get; set; }

    public decimal? Discount { get; set; }

    public int? AvailableStock { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblCategory? Category { get; set; }

    public virtual TblCompany? Company { get; set; }

    public virtual TblApprovalStatus? StatusNavigation { get; set; }

    public virtual TblSubCategory? SubCategory { get; set; }

    public virtual TblSupplier? Supplier { get; set; }

    public virtual ICollection<TblCart> TblCarts { get; set; } = new List<TblCart>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual ICollection<TblProductMedia> TblProductMedia { get; set; } = new List<TblProductMedia>();

    public virtual ICollection<TblProductRating> TblProductRatings { get; set; } = new List<TblProductRating>();

    public virtual ICollection<TblProductReview> TblProductReviews { get; set; } = new List<TblProductReview>();
}
