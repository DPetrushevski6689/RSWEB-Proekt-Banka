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
    public class KompaniskiSmetkiController : Controller
    {
        private readonly BankaContext _context;

        public KompaniskiSmetkiController(BankaContext context)
        {
            _context = context;
        }

        // GET: KompaniskiSmetki
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string searchBankarskiBroj)
        {
            IQueryable<KompaniskaSmetka> smetki = _context.KompaniskaSmetka.AsQueryable();
            

            if (!string.IsNullOrEmpty(searchBankarskiBroj))
            {
                smetki = smetki.Where(s => s.bankarskiBroj.Contains(searchBankarskiBroj));
            }

           

            smetki = smetki.Include(s => s.Firma).Include(s => s.vrabotenKoordinator).ThenInclude(s => s.vrabotenKoordinator);

            var viewmodel = new KompaniskiSmetkiFilter
            {
                KompaniskiSmetki = await smetki.ToListAsync(),
                
            };


            return View(viewmodel);
        }


        // GET: KompaniskiSmetki/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kompaniskaSmetka = await _context.KompaniskaSmetka
                .Include(k => k.vrabotenKoordinator).ThenInclude(k => k.vrabotenKoordinator)
                .Include(k => k.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kompaniskaSmetka == null)
            {
                return NotFound();
            }

            return View(kompaniskaSmetka);
        }

        // GET: KompaniskiSmetki/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName");
            return View();
        }

        // POST: KompaniskiSmetki/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,bankarskiBroj,paricnaSostojba,dataIzdavanje,tip,firmaId")] KompaniskaSmetka kompaniskaSmetka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kompaniskaSmetka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName", kompaniskaSmetka.firmaId);
            return View(kompaniskaSmetka);
        }

        // GET: KompaniskiSmetki/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kompaniskaSmetka = await _context.KompaniskaSmetka.FindAsync(id);
            if (kompaniskaSmetka == null)
            {
                return NotFound();
            }
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName", kompaniskaSmetka.firmaId);
            return View(kompaniskaSmetka);
        }

        // POST: KompaniskiSmetki/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,bankarskiBroj,paricnaSostojba,dataIzdavanje,tip,firmaId")] KompaniskaSmetka kompaniskaSmetka)
        {
            if (id != kompaniskaSmetka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kompaniskaSmetka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KompaniskaSmetkaExists(kompaniskaSmetka.Id))
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
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName", kompaniskaSmetka.firmaId);
            return View(kompaniskaSmetka);
        }

        // GET: KompaniskiSmetki/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kompaniskaSmetka = await _context.KompaniskaSmetka
                .Include(k => k.vrabotenKoordinator).ThenInclude(k => k.vrabotenKoordinator)
                .Include(k => k.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kompaniskaSmetka == null)
            {
                return NotFound();
            }

            return View(kompaniskaSmetka);
        }

        // POST: KompaniskiSmetki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kompaniskaSmetka = await _context.KompaniskaSmetka.FindAsync(id);
            _context.KompaniskaSmetka.Remove(kompaniskaSmetka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KompaniskaSmetkaExists(int id)
        {
            return _context.KompaniskaSmetka.Any(e => e.Id == id);
        }
    }
}
