using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class EmployeeFirms
    {
        public int Id { get; set; }
        public int employeeId { get; set; }
        [Display(Name = "Координатор")]
        public Employee vrabotenKoordinator { get; set; }
        public int kompaniskaSmetkaId { get; set; }
        [Display(Name = "Компаниска сметка")]
        public KompaniskaSmetka kompaniskaSmetka { get; set; }

        
        [Display(Name = "Тип на сметка(активна,блокирана)")]
        public string tip { get; set; }
    }
}
