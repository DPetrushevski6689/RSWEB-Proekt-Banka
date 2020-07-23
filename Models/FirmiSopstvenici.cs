using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class FirmiSopstvenici
    {
        public int Id { get; set; }
        public int sopstvenikId { get; set; }
        [Display(Name = "Сопственик")]
        public Korisnik Sopstvenik { get; set; }
        public int firmaId { get; set; }
        [Display(Name = "Име на фирма")]
        public Firma Firma { get; set; }
    }
}
