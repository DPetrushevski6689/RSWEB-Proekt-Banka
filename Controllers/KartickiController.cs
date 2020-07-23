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
using Microsoft.AspNetCore.Authorization;

namespace Banka.Controllers
{
    public class KartickiController : Controller
    {
        private readonly BankaContext _context;

        public KartickiController(BankaContext context)
        {
            _context = context;
        }

        // GET: Karticki
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string searchBrojKarticka, string searchTipKarticka)
        {
            IQueryable<Karticka> karticki = _context.Karticka.AsQueryable();
            IQueryable<string> tipKartici = _context.Karticka.OrderBy(k => k.tipNaKarticka)
                .Select(k => k.tipNaKarticka).Distinct();

            if (!string.IsNullOrEmpty(searchBrojKarticka))
            {
                karticki = karticki.Where(k => k.brojNaKarticka.Contains(searchBrojKarticka));
            }
            if(!string.IsNullOrEmpty(searchTipKarticka))
            {
                karticki = karticki.Where(k => k.tipNaKarticka == searchTipKarticka);
            }

            karticki = karticki.Include(k => k.korisnickSmetka);

            var viewmodel = new KartickiFilter
            {
                Karticki = await karticki.ToListAsync(),
                tipoviKarticki = new SelectList(await tipKartici.ToListAsync())
            };

            //var bankaContext = _context.Karticka.Include(k => k.korisnickSmetka);
            return View(viewmodel);
        }

        // GET: Karticki/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karticka = await _context.Karticka
                .Include(k => k.korisnickSmetka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (karticka == null)
            {
                return NotFound();
            }

            return View(karticka);
        }

        // GET: Karticki/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["korisnickaSmetkaId"] = new SelectList(_context.KorisnickaSmetka, "Id", "bankarskiBroj");
            return View();
        }

        // POST: Karticki/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,brojNaKarticka,tipNaKarticka,paricnaSostojba,korisnickaSmetkaId")] Karticka karticka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(karticka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnickaSmetkaId"] = new SelectList(_context.KorisnickaSmetka, "Id", "bankarskiBroj", karticka.korisnickaSmetkaId);
            return View(karticka);
        }

        // GET: Karticki/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karticka = await _context.Karticka.FindAsync(id);
            if (karticka == null)
            {
                return NotFound();
            }
            ViewData["korisnickaSmetkaId"] = new SelectList(_context.KorisnickaSmetka, "Id", "bankarskiBroj", karticka.korisnickaSmetkaId);
            return View(karticka);
        }

        // POST: Karticki/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,brojNaKarticka,tipNaKarticka,paricnaSostojba,korisnickaSmetkaId")] Karticka karticka)
        {
            if (id != karticka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(karticka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KartickaExists(karticka.Id))
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
            ViewData["korisnickaSmetkaId"] = new SelectList(_context.KorisnickaSmetka, "Id", "bankarskiBroj", karticka.korisnickaSmetkaId);
            return View(karticka);
        }

        // GET: Karticki/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karticka = await _context.Karticka
                .Include(k => k.korisnickSmetka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (karticka == null)
            {
                return NotFound();
            }

            return View(karticka);
        }

        // POST: Karticki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var karticka = await _context.Karticka.FindAsync(id);
            _context.Karticka.Remove(karticka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KartickaExists(int id)
        {
            return _context.Karticka.Any(e => e.Id == id);
        }
    }
}
