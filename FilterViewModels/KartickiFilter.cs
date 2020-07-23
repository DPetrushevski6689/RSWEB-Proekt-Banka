using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FilterViewModels
{
    public class KartickiFilter
    {
        public IList<Karticka> Karticki { get; set; }
        public string searchBrojKarticka { get; set; }
        public SelectList tipoviKarticki { get; set; }
        public string searchTipKarticka { get; set; }
    }
}
