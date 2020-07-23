using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Име")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Презиме")]
        public string lastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Датум на раѓање")]
        public DateTime birthDate { get; set; }
        [Required]
        [Display(Name = "Адреса")]
        public string Address { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                TimeSpan span = DateTime.Now - birthDate;
                double years = (double)span.TotalDays / 365.2425;
                return (int)years;
            }
        }

        public string FullName {
            get
            {
                return String.Format("{0} {1}", firstName, lastName);
            }
        }

        [Display(Name = "Фирми")]
        public ICollection<FirmiSopstvenici> Firmi { get; set; }

        [Display(Name = "Кориснички Сметки")]
        public ICollection<KorisnickaSmetka> KorisnickiSmetki { get; set; }
    }
}
