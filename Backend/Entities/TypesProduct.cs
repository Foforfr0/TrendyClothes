using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class TypesProduct
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
