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
    public class KorisnickiSmetkiController : Controller
    {
        private readonly BankaContext _context;

        public KorisnickiSmetkiController(BankaContext context)
        {
            _context = context;
        }

        // GET: KorisnickiSmetki
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string searchBankarskiBroj, string tipSmetka)
        {
            IQueryable<KorisnickaSmetka> smetki = _context.KorisnickaSmetka.AsQueryable();
            IQueryable<string> tipoviSmetki = _context.KorisnickaSmetka.OrderBy(s => s.tip)
                .Select(s => s.tip).Distinct();

            if (!string.IsNullOrEmpty(searchBankarskiBroj))
            {
                smetki = smetki.Where(s => s.bankarskiBroj.Contains(searchBankarskiBroj));
            }

            if (!string.IsNullOrEmpty(tipSmetka))
            {
                smetki = smetki.Where(s => s.tip == tipSmetka);
            }

            smetki = smetki.Include(s => s.Korisnik).Include(s => s.Karticki);

            var viewmodel = new KorisnickiSmetkiFilter
            {
                KorisnickiSmetki = await smetki.ToListAsync(),
                tipoviSmetka = new SelectList(await tipoviSmetki.ToListAsync())
            };

           
            return View(viewmodel);
        }

        // GET: KorisnickiSmetki/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnickaSmetka = await _context.KorisnickaSmetka
                .Include(k => k.Korisnik)
                .Include(k => k.Karticki)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnickaSmetka == null)
            {
                return NotFound();
            }

            return View(korisnickaSmetka);
        }

        // GET: KorisnickiSmetki/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "FullName");
            return View();
        }

        // POST: KorisnickiSmetki/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,bankarskiBroj,paricnaSostojba,dataIzdavanje,tip,korisnikId")] KorisnickaSmetka korisnickaSmetka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnickaSmetka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "FullName", korisnickaSmetka.korisnikId);
            return View(korisnickaSmetka);
        }

        // GET: KorisnickiSmetki/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnickaSmetka = await _context.KorisnickaSmetka.FindAsync(id);
            if (korisnickaSmetka == null)
            {
                return NotFound();
            }
            ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "FullName", korisnickaSmetka.korisnikId);
            return View(korisnickaSmetka);
        }

        // POST: KorisnickiSmetki/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,bankarskiBroj,paricnaSostojba,dataIzdavanje,tip,korisnikId")] KorisnickaSmetka korisnickaSmetka)
        {
            if (id != korisnickaSmetka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnickaSmetka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnickaSmetkaExists(korisnickaSmetka.Id))
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
            ViewData["korisnikId"] = new SelectList(_context.Korisnik, "Id", "FullName", korisnickaSmetka.korisnikId);
            return View(korisnickaSmetka);
        }

        // GET: KorisnickiSmetki/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnickaSmetka = await _context.KorisnickaSmetka
                .Include(k => k.Karticki)
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnickaSmetka == null)
            {
                return NotFound();
            }

            return View(korisnickaSmetka);
        }

        // POST: KorisnickiSmetki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnickaSmetka = await _context.KorisnickaSmetka.FindAsync(id);
            _context.KorisnickaSmetka.Remove(korisnickaSmetka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnickaSmetkaExists(int id)
        {
            return _context.KorisnickaSmetka.Any(e => e.Id == id);
        }
    }
}
