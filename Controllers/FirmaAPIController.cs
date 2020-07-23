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
    public class FirmaAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public FirmaAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/FirmaAPI
        [HttpGet]
        public IList<Firma> GetFirma(string searchfirmName)
        {
            IQueryable<Firma> firmi = _context.Firma.AsQueryable();

            if (!string.IsNullOrEmpty(searchfirmName))
            {
                firmi = firmi.Where(f => f.firmName.Contains(searchfirmName));
            }

            return firmi.ToList();
        }

        // GET: api/FirmaAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFirma([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firma = await _context.Firma.FindAsync(id);

            if (firma == null)
            {
                return NotFound();
            }

            return Ok(firma);
        }

        // PUT: api/FirmaAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFirma([FromRoute] int id, [FromBody] Firma firma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != firma.Id)
            {
                return BadRequest();
            }

            _context.Entry(firma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FirmaExists(id))
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

        // POST: api/FirmaAPI
        [HttpPost]
        public async Task<IActionResult> PostFirma([FromBody] Firma firma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Firma.Add(firma);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFirma", new { id = firma.Id }, firma);
        }

        // DELETE: api/FirmaAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFirma([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firma = await _context.Firma.FindAsync(id);
            if (firma == null)
            {
                return NotFound();
            }

            _context.Firma.Remove(firma);
            await _context.SaveChangesAsync();

            return Ok(firma);
        }

        private bool FirmaExists(int id)
        {
            return _context.Firma.Any(e => e.Id == id);
        }
    }
}