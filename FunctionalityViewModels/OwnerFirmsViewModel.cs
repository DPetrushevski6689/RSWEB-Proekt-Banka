using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FunctionalityViewModels
{
    public class OwnerFirmsViewModel
    {
        [Required]
        [Display(Name = "Сопственик")]
        public int sopstvenikId { get; set; }

        [Required]
        [Display(Name = "Фирми")]
        public IList<int> firmaIds { get; set; }

        public SelectList Firmi { get; set; }
        public SelectList Sopstvenici { get; set; }
    }
}
