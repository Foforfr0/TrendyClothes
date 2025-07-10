using System;
using System.Collections.Generic;

namespace AuctionStatistics.Entities;

public partial class User_Address
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? AddressId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Address? Address { get; set; }

    public virtual User? User { get; set; }
}
