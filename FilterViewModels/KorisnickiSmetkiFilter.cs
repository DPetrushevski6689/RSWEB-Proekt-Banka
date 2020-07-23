using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FilterViewModels
{
    public class KorisnickiSmetkiFilter
    {
        public IList<KorisnickaSmetka> KorisnickiSmetki { get; set; }
        public string searchBankarskiBroj { get; set; }
        public SelectList tipoviSmetka { get; set; }
        public string tipSmetka { get; set; }
    }
}
