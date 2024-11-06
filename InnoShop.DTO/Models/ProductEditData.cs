using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnoShop.DTO.Models
{
    public class ProductEditData
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
