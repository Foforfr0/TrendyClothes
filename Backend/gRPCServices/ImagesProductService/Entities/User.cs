using System;
using System.Collections.Generic;

namespace ImagesProductService.Entities;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string AreaCode { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? TwoFactorCode { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<AuctionsProduct> AuctionsProducts { get; set; } = new List<AuctionsProduct>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<QAProduct> QAProducts { get; set; } = new List<QAProduct>();

    public virtual RolesUser Role { get; set; } = null!;

    public virtual ICollection<User_Address> User_Addresses { get; set; } = new List<User_Address>();
}
