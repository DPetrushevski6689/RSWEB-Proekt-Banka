using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FilterViewModels
{
    public class KompaniskiSmetkiFilter
    {
        public IList<KompaniskaSmetka> KompaniskiSmetki { get; set; }
        public string searchBankarskiBroj { get; set; }
       
    }
}
