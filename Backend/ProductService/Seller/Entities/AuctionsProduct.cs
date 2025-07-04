using System;
using System.Collections.Generic;

namespace ProductSellerService.Entities;

public partial class AuctionsProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? FirstPrice { get; set; }

    public decimal? Bid { get; set; }

    public decimal? LastPrice { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime DateEnd { get; set; }

    public string Description { get; set; } = null!;

    public int SellerId { get; set; }

    public int ProductId { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<BidsAuction> BidsAuctions { get; set; } = new List<BidsAuction>();

    public virtual ICollection<PhotosAuction> PhotosAuctions { get; set; } = new List<PhotosAuction>();

    public virtual Product Product { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;

    public virtual StatusesAuction Status { get; set; } = null!;
}
