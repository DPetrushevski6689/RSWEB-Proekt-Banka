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
    public class KorisnikAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public KorisnikAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/KorisnikAPI
        [HttpGet]
        public List<Korisnik> GetKorisnik(string searchfirstName, string searchlastName)
        {
            IQueryable<Korisnik> korisnici = _context.Korisnik.AsQueryable();
            if (!string.IsNullOrEmpty(searchfirstName))
            {
                korisnici = korisnici.Where(c => c.firstName.Contains(searchfirstName));
            }

            if (!string.IsNullOrEmpty(searchlastName))
            {
                korisnici = korisnici.Where(c => c.lastName.Contains(searchlastName));
            }
            return korisnici.ToList();
        }

        // GET: api/KorisnikAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKorisnik([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var korisnik = await _context.Korisnik.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return Ok(korisnik);
        }

        // PUT: api/KorisnikAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnik([FromRoute] int id, [FromBody] KorisniciFirmiEdit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Sopstvenik.Id)
            {
                return BadRequest();
            }

            _context.Entry(model.Sopstvenik).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                IEnumerable<int> listFirmi = model.SelectedFirmi;
                IQueryable<FirmiSopstvenici> toBeRemoved = _context.FirmiSopstvenici.Where(s => !listFirmi.Contains(s.firmaId)
                && s.sopstvenikId == id);
                _context.FirmiSopstvenici.RemoveRange(toBeRemoved);

                IEnumerable<int> existFirmi = _context.FirmiSopstvenici.Where(s => listFirmi.Contains(s.firmaId)
                && s.sopstvenikId == id).Select(s => s.firmaId);

                IEnumerable<int> newFirmi = listFirmi.Where(e => !existFirmi.Contains(e));

                foreach (int firmId in newFirmi)
                {
                    _context.FirmiSopstvenici.Add(new FirmiSopstvenici { firmaId = firmId, sopstvenikId = id });
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisnikExists(id))
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

        // POST: api/KorisnikAPI
        [HttpPost]
        public async Task<IActionResult> PostKorisnik([FromBody] Korisnik korisnik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Korisnik.Add(korisnik);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKorisnik", new { id = korisnik.Id }, korisnik);
        }

        // DELETE: api/KorisnikAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnik([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var korisnik = await _context.Korisnik.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }

            _context.Korisnik.Remove(korisnik);
            await _context.SaveChangesAsync();

            return Ok(korisnik);
        }

        private bool KorisnikExists(int id)
        {
            return _context.Korisnik.Any(e => e.Id == id);
        }
    }
}