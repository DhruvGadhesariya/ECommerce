using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class TblProductMedia
{
    public long MediaId { get; set; }

    public string? MediaName { get; set; }

    public string? MediaPath { get; set; }

    public string MediaType { get; set; } = null!;

    public bool? Default { get; set; }

    public long? ProductId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TblProduct? Product { get; set; }
}
