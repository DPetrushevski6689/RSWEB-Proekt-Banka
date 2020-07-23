using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.EditViewModels
{
    public class KorisniciFirmiEdit
    {
        public Korisnik Sopstvenik { get; set; }
        public IEnumerable<int> SelectedFirmi { get; set; }
        public IEnumerable<SelectListItem> FirmiList { get; set; }
    }
}
