using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class AuctionsProduct
{
    public int Id { get; set; }

    public int Number { get; set; }

    public decimal? FirstPrice { get; set; }

    public decimal? MinBid { get; set; }

    public decimal? LastPrice { get; set; }

    public int ProductId { get; set; }

    public int StatusId { get; set; }

    public int? BuyerId { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual StatusesAuction Status { get; set; } = null!;
}
