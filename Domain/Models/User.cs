using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Email { get; set; }

    public bool? Admin { get; set; }

    public int UserTypeId { get; set; }

    public int LocalityId { get; set; }

    public virtual Locality Locality { get; set; } = null!;

    public virtual ICollection<Product> ProductBuyers { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUsers { get; set; } = new List<Product>();

    public virtual UserType UserType { get; set; } = null!;
}
