using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.DTO.Models
{
    public class ProductEditDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int ProdTypeId { get; set; }
        public DateTime CreationDate {  get; set; }
        public bool Public { get; set; }
        public int UserId { get; set; }
        public bool Sold { get; set; }
        public int? BuyerId { get; set; }
        public string ProdAttribs { get; set; } 
    }
}
