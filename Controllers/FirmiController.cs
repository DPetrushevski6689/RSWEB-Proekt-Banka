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
    public class FirmiController : Controller
    {
        private readonly BankaContext _context;

        public FirmiController(BankaContext context)
        {
            _context = context;
        }

        // GET: Firmi
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string searchfirmName)
        {
            IQueryable<Firma> firmi = _context.Firma.AsQueryable();

            if (!string.IsNullOrEmpty(searchfirmName))
            {
                firmi = firmi.Where(f => f.firmName.Contains(searchfirmName));
            }

            firmi = firmi.Include(f => f.Sopstvenici).ThenInclude(f => f.Sopstvenik).Include(f => f.kompaniskiSmetki);

            var viewmodel = new FirmiFilter
            {
                Firmi = await firmi.ToListAsync()
            };

            return View(viewmodel);
        }

        // GET: Firmi/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firma = await _context.Firma
                .Include(f => f.kompaniskiSmetki)
                .Include(f => f.Sopstvenici).ThenInclude(f => f.Sopstvenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firma == null)
            {
                return NotFound();
            }

            return View(firma);
        }

        // GET: Firmi/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Firmi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,firmName,dataOsnovanje,Address")] Firma firma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(firma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(firma);
        }

        // GET: Firmi/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firma = await _context.Firma.FindAsync(id);
            if (firma == null)
            {
                return NotFound();
            }
            return View(firma);
        }

        // POST: Firmi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,firmName,dataOsnovanje,Address")] Firma firma)
        {
            if (id != firma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmaExists(firma.Id))
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
            return View(firma);
        }

        // GET: Firmi/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firma = await _context.Firma
                .Include(f => f.kompaniskiSmetki)
                .Include(f => f.Sopstvenici).ThenInclude(f => f.Sopstvenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firma == null)
            {
                return NotFound();
            }

            return View(firma);
        }

        // POST: Firmi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firma = await _context.Firma.FindAsync(id);
            _context.Firma.Remove(firma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FirmaExists(int id)
        {
            return _context.Firma.Any(e => e.Id == id);
        }
    }
}
