using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Име на вработен")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Презиме на вработен")]
        public string lastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Датум на раѓање")]
        public DateTime birthDate { get; set; }

        [Required]
        [Display(Name = "Позиција")]
        public string Position { get; set; }

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

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", firstName, lastName);
            }
        }

        [Display(Name = "Сметки за координација")]
        public ICollection<EmployeeFirms> KompaniskiSmetki { get; set; }

    }
}
