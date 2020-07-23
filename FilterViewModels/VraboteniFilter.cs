using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.FilterViewModels
{
    public class VraboteniFilter
    {
        public IList<Employee> Vraboteni { get; set; }
        public string searchFirstName { get; set; }
        public string searchLastName { get; set; }
        public SelectList Pozicii { get; set; }
        public string searchPosition { get; set; }
    }
}
