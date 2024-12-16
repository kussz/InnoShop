using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoShop.Domain.Models;

public partial class ProdAttrib
{
    public int Id { get; set; }
    [MaxLength(40,ErrorMessage ="Максимальная длина названия 40 символов.")]
    [Required(ErrorMessage ="Тег не может быть пустым.")]

    public string Name { get; set; } = null!;

    public int ProdId { get; set; }
    [JsonIgnore]
    public virtual Product Prod { get; set; } = null!;
}
