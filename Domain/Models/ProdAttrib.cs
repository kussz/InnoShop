using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class ProdAttrib
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProdId { get; set; }

    public virtual Product Prod { get; set; } = null!;
}
