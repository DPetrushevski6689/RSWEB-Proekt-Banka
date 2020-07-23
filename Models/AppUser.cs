using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class AppUser:IdentityUser
    {
        [Display(Name = "Улога")]
        public string Role { get; set; }

        public int? VrabotenId { get; set; }
        [Display(Name = "Вработен")]
        [ForeignKey("VrabotenId")]
        public Employee Vraboten { get; set; }

        public int? KorisnikId { get; set; }
        [Display(Name = "Корисник")]
        [ForeignKey("KorisnikId")]
        public Korisnik Korisnik { get; set; }
    }
}
