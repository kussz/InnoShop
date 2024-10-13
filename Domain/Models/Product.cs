using System;
using System.Collections.Generic;

namespace InnoShop.Domain.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public int ProdTypeId { get; set; }

    public bool Public { get; set; }

    public int UserId { get; set; }

    public DateTime CreationDate { get; set; }

    public bool Sold { get; set; }

    public int? BuyerId { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual ICollection<ProdAttrib> ProdAttribs { get; set; } = new List<ProdAttrib>();

    public virtual ProdType ProdType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
