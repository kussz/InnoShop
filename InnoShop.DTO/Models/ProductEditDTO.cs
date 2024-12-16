using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.DTO.Models
{
    public class ProductEditDTO
    {
        public int Id {  get; set; }
        [Required(ErrorMessage ="Название не указано.")]
        [MaxLength(40,ErrorMessage ="Максимальная длина названия 40 символов.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Описание не указано.")]
        public string Description { get; set; }
        [Range(0,2000,ErrorMessage ="Стоимость продукта должна находиться в пределах от 0 до 2000 р.")]
        [Required(ErrorMessage ="Стоимость продукта не указана.")]
        public decimal Cost { get; set; }
        public int ProdTypeId { get; set; }
        public DateTime CreationDate {  get; set; }
        public bool Public { get; set; }
        public int UserId { get; set; }
        public bool Sold { get; set; } = false;
        public int? BuyerId { get; set; } = null;
        public string ProdAttribs { get; set; } 
    }
}
