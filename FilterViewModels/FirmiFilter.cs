using Banka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FilterViewModels
{
    public class FirmiFilter
    {
        public IList<Firma> Firmi { get; set; }
        public string searchfirmName { get; set; }
    }
}
