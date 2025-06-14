using System;
using System.Collections.Generic;

namespace ProductService.Entities;

public partial class StatusesProduct
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
