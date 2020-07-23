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
    public class KartickiAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public KartickiAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/KartickiAPI
        [HttpGet]
        public IList<Karticka> GetKarticka(string searchBrojKarticka, string searchTipKarticka)
        {
            IQueryable<Karticka> karticki = _context.Karticka.AsQueryable();
            IQueryable<string> tipKartici = _context.Karticka.OrderBy(k => k.tipNaKarticka)
                .Select(k => k.tipNaKarticka).Distinct();

            if (!string.IsNullOrEmpty(searchBrojKarticka))
            {
                karticki = karticki.Where(k => k.brojNaKarticka.Contains(searchBrojKarticka));
            }
            if (!string.IsNullOrEmpty(searchTipKarticka))
            {
                karticki = karticki.Where(k => k.tipNaKarticka == searchTipKarticka);
            }

            return karticki.ToList();
        }

        // GET: api/KartickiAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKarticka([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var karticka = await _context.Karticka.FindAsync(id);

            if (karticka == null)
            {
                return NotFound();
            }

            return Ok(karticka);
        }

        // PUT: api/KartickiAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKarticka([FromRoute] int id, [FromBody] Karticka karticka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != karticka.Id)
            {
                return BadRequest();
            }

            _context.Entry(karticka).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KartickaExists(id))
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

        // POST: api/KartickiAPI
        [HttpPost]
        public async Task<IActionResult> PostKarticka([FromBody] Karticka karticka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Karticka.Add(karticka);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKarticka", new { id = karticka.Id }, karticka);
        }

        // DELETE: api/KartickiAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKarticka([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var karticka = await _context.Karticka.FindAsync(id);
            if (karticka == null)
            {
                return NotFound();
            }

            _context.Karticka.Remove(karticka);
            await _context.SaveChangesAsync();

            return Ok(karticka);
        }

        private bool KartickaExists(int id)
        {
            return _context.Karticka.Any(e => e.Id == id);
        }
    }
}