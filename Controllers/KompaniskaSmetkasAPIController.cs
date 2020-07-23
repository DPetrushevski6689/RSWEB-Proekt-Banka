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
    public class KompaniskaSmetkasAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public KompaniskaSmetkasAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/KompaniskaSmetkasAPI
        [HttpGet]
        public IList<KompaniskaSmetka> GetKompaniskaSmetka(string searchBankarskiBroj)
        {
            IQueryable<KompaniskaSmetka> smetki = _context.KompaniskaSmetka.AsQueryable();


            if (!string.IsNullOrEmpty(searchBankarskiBroj))
            {
                smetki = smetki.Where(s => s.bankarskiBroj.Contains(searchBankarskiBroj));
            }

            return smetki.ToList();
        }

        // GET: api/KompaniskaSmetkasAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKompaniskaSmetka([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kompaniskaSmetka = await _context.KompaniskaSmetka.FindAsync(id);

            if (kompaniskaSmetka == null)
            {
                return NotFound();
            }

            return Ok(kompaniskaSmetka);
        }

        // PUT: api/KompaniskaSmetkasAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKompaniskaSmetka([FromRoute] int id, [FromBody] KompaniskaSmetka kompaniskaSmetka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kompaniskaSmetka.Id)
            {
                return BadRequest();
            }

            _context.Entry(kompaniskaSmetka).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KompaniskaSmetkaExists(id))
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

        // POST: api/KompaniskaSmetkasAPI
        [HttpPost]
        public async Task<IActionResult> PostKompaniskaSmetka([FromBody] KompaniskaSmetka kompaniskaSmetka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.KompaniskaSmetka.Add(kompaniskaSmetka);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKompaniskaSmetka", new { id = kompaniskaSmetka.Id }, kompaniskaSmetka);
        }

        // DELETE: api/KompaniskaSmetkasAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKompaniskaSmetka([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kompaniskaSmetka = await _context.KompaniskaSmetka.FindAsync(id);
            if (kompaniskaSmetka == null)
            {
                return NotFound();
            }

            _context.KompaniskaSmetka.Remove(kompaniskaSmetka);
            await _context.SaveChangesAsync();

            return Ok(kompaniskaSmetka);
        }

        private bool KompaniskaSmetkaExists(int id)
        {
            return _context.KompaniskaSmetka.Any(e => e.Id == id);
        }
    }
}