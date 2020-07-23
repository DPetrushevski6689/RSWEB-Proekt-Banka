using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banka.Data;
using Banka.FunctionalityViewModels;
using Banka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banka.Controllers
{
    [Authorize(Roles = "Administrator,Vraboten")]
    public class VrabotenController : Controller
    {
        private readonly BankaContext _context;
        private readonly UserManager<AppUser> userManager;
        public VrabotenController(BankaContext context, UserManager<AppUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        [Authorize(Roles = "Administrator,Vraboten")]
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if(curruser.VrabotenId !=null)
                {
                    return RedirectToAction(nameof(Index), new { id = curruser.VrabotenId });
                }
                else
                {
                    return NotFound();
                }
               
            }

            Employee vraboten = await _context.Employee
                .Include(m => m.KompaniskiSmetki).ThenInclude(m => m.kompaniskaSmetka)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(vraboten == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.GetUserAsync(User);
            if(vraboten.Id != user.VrabotenId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            return View(vraboten);
        }

        //[Authorize(Roles = "Administrator,Vraboten")]
        [Authorize(Roles = "Administrator,Vraboten")]
        public async Task<IActionResult> editSmetka(int? id) //id na kompaniskasmetka
        {
            if(id == null)
            {
                return NotFound();
            }

            var zapis = await _context.EmployeeFirms
                .Include(m => m.kompaniskaSmetka)
                .Include(m => m.vrabotenKoordinator)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(zapis == null)
            {
                return NotFound();
            }

            var viewmodel = new EditSmetkaViewModel
            {
                Zapis = zapis
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editSmetka(int id, EditSmetkaViewModel entry)
        {
            if(entry.Zapis.Id != id)
            {
                return NotFound();
            }

            var zapis = await _context.EmployeeFirms
                .Include(m => m.kompaniskaSmetka)
                .Include(m => m.vrabotenKoordinator)
                .FirstOrDefaultAsync(m => m.Id == id);


            if(zapis == null)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                zapis.tip = entry.Zapis.tip;
                _context.Update(zapis);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(editSmetka));
        }

    }
}