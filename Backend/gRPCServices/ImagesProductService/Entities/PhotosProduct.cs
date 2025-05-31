using System;
using System.Collections.Generic;

namespace ImagesProductService.Entities;

public partial class PhotosProduct
{
    public int Id { get; set; }

    public byte[] Photo { get; set; } = null!;

    public int ProductId { get; set; }

    public string Mime { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
