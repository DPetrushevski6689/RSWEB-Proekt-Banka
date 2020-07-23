using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FunctionalityViewModels
{
    public class VrabotenSmetkiViewModel
    {
        [Required]
        [Display(Name = "Вработен")]
        public int vrabotenId { get; set; }
        [Required]
        [Display(Name = "Компаниски сметки")]
        public IList<int> kompaniskiSmetkiIds { get; set; }

        public SelectList kompaniskiSmetki { get; set; }

        public SelectList Vraboteni { get; set; }
    }
}
