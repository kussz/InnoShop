using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace InnoShop.Domain.Models;

public partial class User : IdentityUser<int>
{

    public int UserTypeId { get; set; }


    public int? LocalityId { get; set; }

    public virtual Locality Locality { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Product> ProductsBuyer { get; set; } = new List<Product>();
    [JsonIgnore]

    public virtual ICollection<Product> ProductsUser { get; set; } = new List<Product>();

    public virtual UserType UserType { get; set; } = null!;
}
