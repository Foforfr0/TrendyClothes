using System;
using System.Collections.Generic;

namespace AuthService.Entities;

public partial class Address
{
    public int Id { get; set; }

    public string Street { get; set; } = null!;

    public string ExtNumber { get; set; } = null!;

    public string? IntNumber { get; set; }

    public string Neighborhood { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<User_Address> User_Addresses { get; set; } = new List<User_Address>();
}
