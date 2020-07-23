using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Banka.Data;
using Banka.Models;

namespace Banka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeFirmsAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public EmployeeFirmsAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeFirmsAPI
        [HttpGet]
        public IList<EmployeeFirms> GetEmployeeFirms()
        {
            IQueryable<EmployeeFirms> zapisi = _context.EmployeeFirms.AsQueryable();

            return zapisi.ToList();
        }

        // GET: api/EmployeeFirmsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeFirms([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeFirms = await _context.EmployeeFirms.FindAsync(id);

            if (employeeFirms == null)
            {
                return NotFound();
            }

            return Ok(employeeFirms);
        }

        // PUT: api/EmployeeFirmsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeFirms([FromRoute] int id, [FromBody] EmployeeFirms employeeFirms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeFirms.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeFirms).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeFirmsExists(id))
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

        // POST: api/EmployeeFirmsAPI
        [HttpPost]
        public async Task<IActionResult> PostEmployeeFirms([FromBody] EmployeeFirms employeeFirms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmployeeFirms.Add(employeeFirms);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeFirms", new { id = employeeFirms.Id }, employeeFirms);
        }

        // DELETE: api/EmployeeFirmsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeFirms([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeFirms = await _context.EmployeeFirms.FindAsync(id);
            if (employeeFirms == null)
            {
                return NotFound();
            }

            _context.EmployeeFirms.Remove(employeeFirms);
            await _context.SaveChangesAsync();

            return Ok(employeeFirms);
        }

        private bool EmployeeFirmsExists(int id)
        {
            return _context.EmployeeFirms.Any(e => e.Id == id);
        }
    }
}