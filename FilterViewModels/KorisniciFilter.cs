using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FilterViewModels
{
    public class KorisniciFilter
    {
        public IList<Korisnik> Korisnici { get; set; }
        public string searchfirstName { get; set; }
        public string searchlastName { get; set; }
    }
}
