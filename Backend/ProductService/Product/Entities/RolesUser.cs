using System;
using System.Collections.Generic;

namespace ProductService.Entities;

public partial class RolesUser
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
