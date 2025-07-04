using System;
using System.Collections.Generic;

namespace AuthService.Entities;

public partial class PhotosAuction
{
    public int Id { get; set; }

    public byte[] Photo { get; set; } = null!;

    public int AuctionId { get; set; }

    public string Mime { get; set; } = null!;

    public virtual AuctionsProduct Auction { get; set; } = null!;
}
