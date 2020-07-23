using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Banka.Data;
using Banka.Models;
using Banka.FilterViewModels;
using Banka.EditViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Banka.Controllers
{
    public class KorisniciController : Controller
    {
        private readonly BankaContext _context;

        public KorisniciController(BankaContext context)
        {
            _context = context;
        }

        // GET: Korisnici
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string searchfirstName, string searchlastName)
        {
            IQueryable<Korisnik> korisnici = _context.Korisnik.AsQueryable();
            if(!string.IsNullOrEmpty(searchfirstName))
            {
                korisnici = korisnici.Where(k => k.firstName.Contains(searchfirstName));
            }
            if (!string.IsNullOrEmpty(searchlastName))
            {
                korisnici = korisnici.Where(k => k.lastName.Contains(searchlastName));
            }
            korisnici = korisnici.Include(k => k.KorisnickiSmetki)
                .Include(k => k.Firmi).ThenInclude(k => k.Firma);

            var viewmodel = new KorisniciFilter
            {
                Korisnici = await korisnici.ToListAsync()
            };
            return View(viewmodel);
        }

        // GET: Korisnici/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .Include(m => m.Firmi).ThenInclude(m => m.Firma)
                .Include(m => m.KorisnickiSmetki)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // GET: Korisnici/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisnici/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,firstName,lastName,birthDate,Address")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(korisnik);
        }

        // GET: Korisnici/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = _context.Korisnik.Where(k => k.Id == id).Include(k => k.Firmi).First();

            if (korisnik == null)
            {
                return NotFound();
            }

            KorisniciFirmiEdit viewmodel = new KorisniciFirmiEdit
            {
                Sopstvenik = korisnik,
                FirmiList = new MultiSelectList(_context.Firma.OrderBy(f => f.firmName), "Id", "firmName"),
                SelectedFirmi = korisnik.Firmi.Select(f => f.firmaId)
            };

            return View(viewmodel);
        }

        // POST: Korisnici/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KorisniciFirmiEdit viewmodel)
        {
            if (id != viewmodel.Sopstvenik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Sopstvenik);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> listFirmi = viewmodel.SelectedFirmi;
                    IQueryable<FirmiSopstvenici> toBeRemoved = _context.FirmiSopstvenici.Where(s => !listFirmi.Contains(s.firmaId)
                    && s.sopstvenikId == id);
                    _context.FirmiSopstvenici.RemoveRange(toBeRemoved);

                    IEnumerable<int> existFirmi = _context.FirmiSopstvenici.Where(s => listFirmi.Contains(s.firmaId)
                    && s.sopstvenikId == id).Select(s => s.firmaId);

                    IEnumerable<int> newFirmi = listFirmi.Where(e => !existFirmi.Contains(e));

                    foreach(int firmId in newFirmi)
                    {
                        _context.FirmiSopstvenici.Add(new FirmiSopstvenici { firmaId = firmId, sopstvenikId = id });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(viewmodel.Sopstvenik.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }

        // GET: Korisnici/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .Include(m => m.Firmi).ThenInclude(m => m.Firma)
                .Include(m => m.KorisnickiSmetki)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: Korisnici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisnik.FindAsync(id);
            _context.Korisnik.Remove(korisnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikExists(int id)
        {
            return _context.Korisnik.Any(e => e.Id == id);
        }
    }
}
