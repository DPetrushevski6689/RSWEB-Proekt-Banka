using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class Firma
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Име на фирма")]
        public string firmName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Датум на основање")]
        public DateTime dataOsnovanje { get; set; }
        [Required]
        [Display(Name = "Адреса на главна канцеларија")]
        public string Address { get; set; }

        [Display(Name = "Сопственици")]
        public ICollection<FirmiSopstvenici> Sopstvenici { get; set; }
        [Display(Name = "Компаниски Сметки")]
        public ICollection<KompaniskaSmetka> kompaniskiSmetki { get; set; }
    }
}
