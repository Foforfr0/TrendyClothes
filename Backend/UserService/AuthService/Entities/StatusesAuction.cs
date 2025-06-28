using System;
using System.Collections.Generic;

namespace AuthService.Entities;

public partial class StatusesAuction
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<AuctionsProduct> AuctionsProducts { get; set; } = new List<AuctionsProduct>();
}
