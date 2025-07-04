using System;
using System.Collections.Generic;

namespace ProductService.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public int? NumberSold { get; set; }

    public decimal? AverageStars { get; set; }

    public string Description { get; set; } = null!;

    public int StockAvailable { get; set; }

    public int SellerId { get; set; }

    public int CategoryId { get; set; }

    public int TypeId { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<AuctionsProduct> AuctionsProducts { get; set; } = new List<AuctionsProduct>();

    public virtual CategoriesProduct Category { get; set; } = null!;

    public virtual ICollection<PhotosProduct> PhotosProducts { get; set; } = new List<PhotosProduct>();

    public virtual ICollection<QAProduct> QAProducts { get; set; } = new List<QAProduct>();

    public virtual User Seller { get; set; } = null!;

    public virtual StatusesProduct Status { get; set; } = null!;

    public virtual TypesProduct Type { get; set; } = null!;
}
