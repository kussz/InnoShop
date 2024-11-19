using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.DTO.Models
{
    public class UserRegisterDTO
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [MinLength(6)]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name ="Тип пользователя")]
        public int UserTypeId { get; set; }
        [Required]
        [Display(Name = "Населённый пункт")]
        public int LocalityId { get; set; }
    }
}
