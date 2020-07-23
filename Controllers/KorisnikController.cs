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
    [Authorize(Roles = "Administrator,Korisnik")]
    public class KorisnikController : Controller
    {
        private readonly BankaContext _context;
        private readonly UserManager<AppUser> userManager;

        public KorisnikController(BankaContext context, UserManager<AppUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if(curruser.KorisnikId!=null)
                {
                    return RedirectToAction(nameof(Index), new { id = curruser.KorisnikId });
                }
                else
                {
                    return NotFound();
                }
            }

            Korisnik korisnik = await _context.Korisnik
                .Include(k => k.KorisnickiSmetki)
                .Include(k => k.Firmi).ThenInclude(k => k.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);

            AppUser user = await userManager.GetUserAsync(User);
            if(id != user.KorisnikId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            if(korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        [Authorize(Roles = "Administrator,Korisnik")]
        public async Task<IActionResult> dodadiSredstva(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            KorisnickaSmetka smetka = await _context.KorisnickaSmetka.FirstOrDefaultAsync(s => s.Id == id);

            var viewmodel = new KorisnikSmetkiViewModel
            {
                Smetka = smetka
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> dodadiSredstva(int? id, KorisnikSmetkiViewModel entry)
        {
           // KorisnickaSmetka input = entry.Smetka;

            if (id != entry.Smetka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                _context.Update(entry.Smetka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entry);
        }
    }
}