using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Banka.Data;
using Banka.Models;
using Banka.EditViewModels;

namespace Banka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public EmployeesAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/EmployeesAPI
        [HttpGet]
        public IList<Employee> GetEmployee(string searchFirstName, string searchLastName, string searchPosition)
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

            return vraboteni.ToList();
        }

        // GET: api/EmployeesAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/EmployeesAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] VraboteniSmetkiEdit viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != viewmodel.Vraboten.Id)
            {
                return BadRequest();
            }

            _context.Entry(viewmodel.Vraboten).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                IEnumerable<int> listSmetki = viewmodel.SelectedKompaniskiSmetki;
                IQueryable<EmployeeFirms> toBeRemoved = _context.EmployeeFirms.Where(e => !listSmetki.Contains(e.kompaniskaSmetkaId)
                 && e.employeeId == id);
                _context.EmployeeFirms.RemoveRange(toBeRemoved);

                IEnumerable<int> existSmetki = _context.EmployeeFirms.Where(e => listSmetki.Contains(e.kompaniskaSmetkaId)
                 && e.employeeId == id).Select(e => e.kompaniskaSmetkaId);
                IEnumerable<int> newSmetki = listSmetki.Where(s => !existSmetki.Contains(s));

                foreach (int smetkaId in newSmetki)
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
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmployeesAPI
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/EmployeesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}