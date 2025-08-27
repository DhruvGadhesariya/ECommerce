using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblProductReview
{
    public long ReviewId { get; set; }

    public string Description { get; set; } = null!;

    public long? UserId { get; set; }

    public long? ProductId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblProduct? Product { get; set; }

    public virtual ICollection<TblReviewMedia> TblReviewMedia { get; set; } = new List<TblReviewMedia>();

    public virtual TblUser? User { get; set; }
}
