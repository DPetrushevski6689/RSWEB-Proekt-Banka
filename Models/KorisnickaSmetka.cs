using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class KorisnickaSmetka
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Банкарски број на сметката")]
        public string bankarskiBroj { get; set; }
        [Required]
        [Display(Name = "Парични средства")]
        public string paricnaSostojba { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Датум на отварање")]
        public DateTime dataIzdavanje { get; set; }
        [Required]
        [Display(Name = "Тип на сметка (активна,блокирана)")]
        public string tip { get; set; }

        public int? korisnikId { get; set; }
        [Display(Name = "Корисник")]
        public Korisnik Korisnik { get; set; }

        [Display(Name = "Платежни картички")]
        public ICollection<Karticka> Karticki { get; set; }

        
    }
}
