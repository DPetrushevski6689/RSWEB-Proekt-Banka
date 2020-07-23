using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class Karticka
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Број на картичка")]
        public string brojNaKarticka { get; set; }

        [Required]
        [Display(Name = "Тип на картичка (кредитна,дебитна)")]
        public string tipNaKarticka { get; set; }
        [Required]
        [Display(Name = "Парични средства")]
        public string paricnaSostojba { get; set; }
        public int korisnickaSmetkaId { get; set; }
        [Display(Name = "Корисничка сметка")]
        public KorisnickaSmetka korisnickSmetka { get; set; }
    }
}
