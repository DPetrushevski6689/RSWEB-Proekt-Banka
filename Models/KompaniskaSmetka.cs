using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class KompaniskaSmetka
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
        

        public int firmaId { get; set; }
        [Display(Name = "Фирма")]
        public Firma Firma { get; set; }

        [Display(Name = "Координатори")]
        public ICollection<EmployeeFirms> vrabotenKoordinator { get; set; }

    }
}
