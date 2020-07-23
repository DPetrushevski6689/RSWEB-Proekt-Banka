using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Banka.Data;
using Banka.Models;
using Banka.FunctionalityViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Banka.Controllers
{
    public class FirmiSopstvenicisController : Controller
    {
        private readonly BankaContext _context;

        public FirmiSopstvenicisController(BankaContext context)
        {
            _context = context;
        }

        // GET: FirmiSopstvenicis
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var bankaContext = _context.FirmiSopstvenici.Include(f => f.Firma).Include(f => f.Sopstvenik);
            return View(await bankaContext.ToListAsync());
        }

        // GET: FirmiSopstvenicis/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmiSopstvenici = await _context.FirmiSopstvenici
                .Include(f => f.Firma)
                .Include(f => f.Sopstvenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firmiSopstvenici == null)
            {
                return NotFound();
            }

            return View(firmiSopstvenici);
        }

        // GET: FirmiSopstvenicis/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName");
            ViewData["sopstvenikId"] = new SelectList(_context.Korisnik, "Id", "FullName");
            return View();
        }

        // POST: FirmiSopstvenicis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,sopstvenikId,firmaId")] FirmiSopstvenici firmiSopstvenici)
        {
            if (ModelState.IsValid)
            {
                _context.Add(firmiSopstvenici);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName", firmiSopstvenici.firmaId);
            ViewData["sopstvenikId"] = new SelectList(_context.Korisnik, "Id", "FullName", firmiSopstvenici.sopstvenikId);
            return View(firmiSopstvenici);
        }

        // GET: FirmiSopstvenicis/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmiSopstvenici = await _context.FirmiSopstvenici.FindAsync(id);
            if (firmiSopstvenici == null)
            {
                return NotFound();
            }
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName", firmiSopstvenici.firmaId);
            ViewData["sopstvenikId"] = new SelectList(_context.Korisnik, "Id", "FullName", firmiSopstvenici.sopstvenikId);
            return View(firmiSopstvenici);
        }

        // POST: FirmiSopstvenicis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,sopstvenikId,firmaId")] FirmiSopstvenici firmiSopstvenici)
        {
            if (id != firmiSopstvenici.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firmiSopstvenici);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmiSopstveniciExists(firmiSopstvenici.Id))
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
            ViewData["firmaId"] = new SelectList(_context.Firma, "Id", "firmName", firmiSopstvenici.firmaId);
            ViewData["sopstvenikId"] = new SelectList(_context.Korisnik, "Id", "FullName", firmiSopstvenici.sopstvenikId);
            return View(firmiSopstvenici);
        }

        // GET: FirmiSopstvenicis/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmiSopstvenici = await _context.FirmiSopstvenici
                .Include(f => f.Firma)
                .Include(f => f.Sopstvenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firmiSopstvenici == null)
            {
                return NotFound();
            }

            return View(firmiSopstvenici);
        }

        // POST: FirmiSopstvenicis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firmiSopstvenici = await _context.FirmiSopstvenici.FindAsync(id);
            _context.FirmiSopstvenici.Remove(firmiSopstvenici);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddOwnerFirms()
        {
            IQueryable<Korisnik> sopstvenici = _context.Korisnik.AsQueryable();
            IQueryable<Firma> firmi = _context.Firma.AsQueryable();

            var viewmodel = new OwnerFirmsViewModel
            {
                Sopstvenici = new SelectList(await sopstvenici.ToListAsync(), "Id", "FullName"),
                Firmi = new SelectList(await firmi.ToListAsync(), "Id", "firmName")
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOwnerFirms(OwnerFirmsViewModel entry)
        {
            Korisnik sopstvenik = await _context.Korisnik.FirstOrDefaultAsync(k => k.Id == entry.sopstvenikId);
            if(sopstvenik == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach(int fId in entry.firmaIds)
                {
                    FirmiSopstvenici zapis = await _context.FirmiSopstvenici
                        .FirstOrDefaultAsync(c => c.sopstvenikId == entry.sopstvenikId && c.firmaId == fId);
                    if(zapis == null)
                    {
                        zapis = new FirmiSopstvenici
                        {
                            sopstvenikId = entry.sopstvenikId,
                            firmaId = fId
                        };
                        _context.Add(zapis);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteOwnerFirms()
        {
            IQueryable<Firma> firmi = _context.Firma.AsQueryable();
            IQueryable<Korisnik> sopstvenici = _context.Korisnik.AsQueryable();

            var viewmodel = new OwnerDeleteFirmsViewModel
            {
                Sopstvenici = new SelectList(await sopstvenici.ToListAsync(), "Id", "FullName"),
                Firmi = new SelectList(await firmi.ToListAsync(), "Id", "firmName")
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOwnerFirms(OwnerDeleteFirmsViewModel entry)
        {
            Korisnik sopstvenik = await _context.Korisnik.FirstOrDefaultAsync(k => k.Id == entry.sopstvenikId);
            if (sopstvenik == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (int fId in entry.firmiIds)
                {
                    FirmiSopstvenici zapisToRemove = await _context.FirmiSopstvenici
                        .FirstOrDefaultAsync(c => c.sopstvenikId == entry.sopstvenikId && c.firmaId == fId);
                    if (zapisToRemove != null)
                    {
                        _context.Remove(zapisToRemove);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FirmiSopstveniciExists(int id)
        {
            return _context.FirmiSopstvenici.Any(e => e.Id == id);
        }
    }
}
