using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class User
    {
        [Display(Name ="Username")]
        public string UserDetails  { get; set; }

        /**[Display(Name = "Password")]
        public string Password { get; set; }**/

        [Display(Name = "Улога")]
        public string Role { get; set; }
        [Display(Name = "ID корисник")]
        public string Id  { get; set; }
        [Display(Name = "Email адреса")]
        public string Email { get; set; }
        [Display(Name = "Хеширана лозинка")]
        public string  PasswordHash { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Нова лозинка")]
        public string NewPassword { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Нова лозинка")]
        [Compare("NewPassword", ErrorMessage = "Лозинките не се совпаѓаат")]
        public string ConfirmPassword { get; set; }
    }
}
