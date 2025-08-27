using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblReviewMedia
{
    public long ReviewMediaId { get; set; }

    public string? ReviewMediaName { get; set; }

    public string? ReviewMediaPath { get; set; }

    public string ReviewMediaType { get; set; } = null!;

    public long? ReviewId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblProductReview? Review { get; set; }
}
