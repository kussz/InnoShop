using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InnoShop.Domain.Models;

public partial class ProdType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
