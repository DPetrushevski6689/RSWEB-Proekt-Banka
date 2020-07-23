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
    public class EmployeesController : Controller
    {
        private readonly BankaContext _context;

        public EmployeesController(BankaContext context)
        {
            _context = context;
        }

        // GET: Employees
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string searchFirstName, string searchLastName, string searchPosition)
        {
            IQueryable<Employee> vraboteni = _context.Employee.AsQueryable();
            IQueryable<string> positionQuery = _context.Employee.OrderBy(e => e.Position)
                .Select(e => e.Position).Distinct();

            if (!string.IsNullOrEmpty(searchFirstName))
            {
                vraboteni = vraboteni.Where(e => e.firstName.Contains(searchFirstName));
            }

            if (!string.IsNullOrEmpty(searchLastName))
            {
                vraboteni = vraboteni.Where(e => e.lastName.Contains(searchLastName));
            }


            if (!string.IsNullOrEmpty(searchPosition))
            {
                vraboteni = vraboteni.Where(e => e.Position == searchPosition);
            }

            vraboteni = vraboteni.Include(e => e.KompaniskiSmetki)
                .ThenInclude(e => e.kompaniskaSmetka);

            var viewmodel = new VraboteniFilter
            {
                Vraboteni = await vraboteni.ToListAsync(),
                Pozicii = new SelectList(await positionQuery.ToListAsync())
            };

            return View(viewmodel);
        }

        // GET: Employees/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.KompaniskiSmetki)
                .ThenInclude(e => e.kompaniskaSmetka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,firstName,lastName,birthDate,Position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _context.Employee.Where(e => e.Id == id).Include(e => e.KompaniskiSmetki).First();
            if (employee == null)
            {
                return NotFound();
            }

            VraboteniSmetkiEdit viewmodel = new VraboteniSmetkiEdit
            {
                Vraboten = employee,
                SmetkiList = new MultiSelectList(_context.KompaniskaSmetka.OrderBy(k => k.bankarskiBroj), "Id", "bankarskiBroj"),
                SelectedKompaniskiSmetki = employee.KompaniskiSmetki.Select(s => s.kompaniskaSmetkaId)
            };

            return View(viewmodel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VraboteniSmetkiEdit viewmodel)
        {
            if (id != viewmodel.Vraboten.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Vraboten);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> listSmetki = viewmodel.SelectedKompaniskiSmetki;
                    IQueryable<EmployeeFirms> toBeRemoved = _context.EmployeeFirms.Where(e => !listSmetki.Contains(e.kompaniskaSmetkaId)
                     && e.employeeId == id);
                    _context.EmployeeFirms.RemoveRange(toBeRemoved);

                    IEnumerable<int> existSmetki = _context.EmployeeFirms.Where(e => listSmetki.Contains(e.kompaniskaSmetkaId)
                     && e.employeeId == id).Select(e => e.kompaniskaSmetkaId);
                    IEnumerable<int> newSmetki = listSmetki.Where(s => !existSmetki.Contains(s));

                    foreach(int smetkaId in newSmetki)
                    {
                        _context.EmployeeFirms.Add(new EmployeeFirms
                        {
                            employeeId = id,
                            kompaniskaSmetkaId = smetkaId
                        });
                    }

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(viewmodel.Vraboten.Id))
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

        // GET: Employees/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.KompaniskiSmetki)
                .ThenInclude(e => e.kompaniskaSmetka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
