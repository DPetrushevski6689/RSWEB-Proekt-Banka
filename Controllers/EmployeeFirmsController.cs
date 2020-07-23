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
    public class EmployeeFirmsController : Controller
    {
        private readonly BankaContext _context;

        public EmployeeFirmsController(BankaContext context)
        {
            _context = context;
        }

        // GET: EmployeeFirms
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var bankaContext = _context.EmployeeFirms.Include(e => e.kompaniskaSmetka).Include(e => e.vrabotenKoordinator);
            return View(await bankaContext.ToListAsync());
        }

        // GET: EmployeeFirms/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeFirms = await _context.EmployeeFirms
                .Include(e => e.kompaniskaSmetka)
                .Include(e => e.vrabotenKoordinator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeFirms == null)
            {
                return NotFound();
            }

            return View(employeeFirms);
        }

        // GET: EmployeeFirms/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["kompaniskaSmetkaId"] = new SelectList(_context.KompaniskaSmetka, "Id", "bankarskiBroj");
            ViewData["employeeId"] = new SelectList(_context.Employee, "Id", "FullName");
            return View();
        }

        // POST: EmployeeFirms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,employeeId,kompaniskaSmetkaId,tip")] EmployeeFirms employeeFirms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeFirms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["kompaniskaSmetkaId"] = new SelectList(_context.KompaniskaSmetka, "Id", "bankarskiBroj", employeeFirms.kompaniskaSmetkaId);
            ViewData["employeeId"] = new SelectList(_context.Employee, "Id", "FullName", employeeFirms.employeeId);
            return View(employeeFirms);
        }

        // GET: EmployeeFirms/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeFirms = await _context.EmployeeFirms.FindAsync(id);
            if (employeeFirms == null)
            {
                return NotFound();
            }
            ViewData["kompaniskaSmetkaId"] = new SelectList(_context.KompaniskaSmetka, "Id", "bankarskiBroj", employeeFirms.kompaniskaSmetkaId);
            ViewData["employeeId"] = new SelectList(_context.Employee, "Id", "FullName", employeeFirms.employeeId);
            return View(employeeFirms);
        }

        // POST: EmployeeFirms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,employeeId,kompaniskaSmetkaId,tip")] EmployeeFirms employeeFirms)
        {
            if (id != employeeFirms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeFirms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeFirmsExists(employeeFirms.Id))
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
            ViewData["kompaniskaSmetkaId"] = new SelectList(_context.KompaniskaSmetka, "Id", "bankarskiBroj", employeeFirms.kompaniskaSmetkaId);
            ViewData["employeeId"] = new SelectList(_context.Employee, "Id", "FullName", employeeFirms.employeeId);
            return View(employeeFirms);
        }

        // GET: EmployeeFirms/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeFirms = await _context.EmployeeFirms
                .Include(e => e.kompaniskaSmetka)
                .Include(e => e.vrabotenKoordinator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeFirms == null)
            {
                return NotFound();
            }

            return View(employeeFirms);
        }

        // POST: EmployeeFirms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeFirms = await _context.EmployeeFirms.FindAsync(id);
            _context.EmployeeFirms.Remove(employeeFirms);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddVrabotenSmetki()
        {
            IQueryable<Employee> vraboteni = _context.Employee.AsQueryable();
            IQueryable<KompaniskaSmetka> smetki = _context.KompaniskaSmetka.AsQueryable();

            var viewmodel = new VrabotenSmetkiViewModel
            {
                Vraboteni = new SelectList(await vraboteni.ToListAsync(), "Id", "FullName"),
                kompaniskiSmetki = new SelectList(await smetki.ToListAsync(), "Id", "bankarskiBroj")
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVrabotenSmetki(VrabotenSmetkiViewModel entry)
        {
            Employee vraboten = await _context.Employee.FirstOrDefaultAsync(k => k.Id == entry.vrabotenId);
            if (vraboten == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (int fId in entry.kompaniskiSmetkiIds)
                {
                    EmployeeFirms zapis = await _context.EmployeeFirms
                        .FirstOrDefaultAsync(c => c.employeeId == entry.vrabotenId && c.kompaniskaSmetkaId == fId);
                    if (zapis == null)
                    {
                        zapis = new EmployeeFirms
                        {
                            employeeId = entry.vrabotenId,
                            kompaniskaSmetkaId = fId
                        };
                        _context.Add(zapis);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeFirmsExists(int id)
        {
            return _context.EmployeeFirms.Any(e => e.Id == id);
        }
    }
}
