using System;
using System.Collections.Generic;

namespace ProfileService.Entities;

public partial class BidsAuction
{
    public int Id { get; set; }

    public decimal Bid { get; set; }

    public DateTime DateBid { get; set; }

    public int BuyerId { get; set; }

    public int AuctionId { get; set; }

    public virtual AuctionsProduct Auction { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;
}
