using System;
using System.Collections.Generic;

namespace ProductSellerService.Entities;

public partial class QAProduct
{
    public int Id { get; set; }

    public decimal? Stars { get; set; }

    public string? Description { get; set; }

    public DateOnly Date { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
