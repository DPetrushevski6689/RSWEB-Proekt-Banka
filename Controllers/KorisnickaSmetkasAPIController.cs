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
    public class KorisnickaSmetkasAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public KorisnickaSmetkasAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/KorisnickaSmetkasAPI
        [HttpGet]
        public IList<KorisnickaSmetka> GetKorisnickaSmetka(string searchBankarskiBroj, string tipSmetka)
        {
            IQueryable<KorisnickaSmetka> smetki = _context.KorisnickaSmetka.AsQueryable();
            IQueryable<string> tipoviSmetki = _context.KorisnickaSmetka.OrderBy(s => s.tip)
                .Select(s => s.tip).Distinct();

            if (!string.IsNullOrEmpty(searchBankarskiBroj))
            {
                smetki = smetki.Where(s => s.bankarskiBroj.Contains(searchBankarskiBroj));
            }

            if (!string.IsNullOrEmpty(tipSmetka))
            {
                smetki = smetki.Where(s => s.tip == tipSmetka);
            }

            return smetki.ToList();
        }

        // GET: api/KorisnickaSmetkasAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKorisnickaSmetka([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var korisnickaSmetka = await _context.KorisnickaSmetka.FindAsync(id);

            if (korisnickaSmetka == null)
            {
                return NotFound();
            }

            return Ok(korisnickaSmetka);
        }

        // PUT: api/KorisnickaSmetkasAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnickaSmetka([FromRoute] int id, [FromBody] KorisnickaSmetka korisnickaSmetka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != korisnickaSmetka.Id)
            {
                return BadRequest();
            }

            _context.Entry(korisnickaSmetka).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisnickaSmetkaExists(id))
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

        // POST: api/KorisnickaSmetkasAPI
        [HttpPost]
        public async Task<IActionResult> PostKorisnickaSmetka([FromBody] KorisnickaSmetka korisnickaSmetka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.KorisnickaSmetka.Add(korisnickaSmetka);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKorisnickaSmetka", new { id = korisnickaSmetka.Id }, korisnickaSmetka);
        }

        // DELETE: api/KorisnickaSmetkasAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnickaSmetka([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var korisnickaSmetka = await _context.KorisnickaSmetka.FindAsync(id);
            if (korisnickaSmetka == null)
            {
                return NotFound();
            }

            _context.KorisnickaSmetka.Remove(korisnickaSmetka);
            await _context.SaveChangesAsync();

            return Ok(korisnickaSmetka);
        }

        private bool KorisnickaSmetkaExists(int id)
        {
            return _context.KorisnickaSmetka.Any(e => e.Id == id);
        }
    }
}