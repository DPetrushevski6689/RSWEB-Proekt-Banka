using Banka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.EditViewModels
{
    public class VraboteniSmetkiEdit
    {
        public Employee Vraboten { get; set; }
        public IEnumerable<int> SelectedKompaniskiSmetki { get; set; }
        public IEnumerable<SelectListItem> SmetkiList { get; set; }
    }
}
