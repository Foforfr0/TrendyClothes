using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class PhotosProduct
{
    public int Id { get; set; }

    public byte[] Photo { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
