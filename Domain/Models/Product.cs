using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InnoShop.Domain.Models;

public partial class Product
{
    public int Id { get; set; }
    [DisplayName("Название")]
    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = null!;
    [DisplayName("Описание")]

    public string? Description { get; set; }
    [DisplayName("Стоимость")]

    public decimal Cost { get; set; }
    [DisplayName("Категория")]
    public int? ProdTypeId { get; set; }
    [DisplayName("Публичный")]
    public bool Public { get; set; }

    public int UserId { get; set; }
    [DisplayName("Дата создания")]
    public DateTime CreationDate { get; set; }
    [DisplayName("Продан")]
    public bool Sold { get; set; }

    public int? BuyerId { get; set; }
    [DisplayName("Покупатель")]
    public virtual User? Buyer { get; set; }
    [DisplayName("Теги")]
    public virtual ICollection<ProdAttrib> ProdAttribs { get; set; } = new List<ProdAttrib>();
    [DisplayName("Категория")]
    public virtual ProdType? ProdType { get; set; } = null!;
    [DisplayName("Продавец")]
    public virtual User User { get; set; } = null!;
}
