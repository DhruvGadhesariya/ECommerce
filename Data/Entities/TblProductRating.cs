using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblProductRating
{
    public long RatingId { get; set; }

    public decimal? Rating { get; set; }

    public long? UserId { get; set; }

    public long? ProductId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblProduct? Product { get; set; }

    public virtual TblUser? User { get; set; }
}
