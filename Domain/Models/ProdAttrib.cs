using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InnoShop.Domain.Models;

public partial class ProdAttrib
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProdId { get; set; }
    [JsonIgnore]
    public virtual Product Prod { get; set; } = null!;
}
