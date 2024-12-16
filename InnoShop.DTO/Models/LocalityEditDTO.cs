using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.DTO.Models
{
    public class LocalityEditDTO
    {
        public int Id { get; set; }
        [MaxLength(40,ErrorMessage ="Максимальная длина названия 40 символов.")]
        public string Name { get; set; }
    }
}
