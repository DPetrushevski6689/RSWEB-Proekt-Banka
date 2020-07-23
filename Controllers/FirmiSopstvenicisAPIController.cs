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
    public class FirmiSopstvenicisAPIController : ControllerBase
    {
        private readonly BankaContext _context;

        public FirmiSopstvenicisAPIController(BankaContext context)
        {
            _context = context;
        }

        // GET: api/FirmiSopstvenicisAPI
        [HttpGet]
        public IList<FirmiSopstvenici> GetFirmiSopstvenici()
        {
            IQueryable<FirmiSopstvenici> zapisi = _context.FirmiSopstvenici.AsQueryable();

            return zapisi.ToList();
        }

        // GET: api/FirmiSopstvenicisAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFirmiSopstvenici([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firmiSopstvenici = await _context.FirmiSopstvenici.FindAsync(id);

            if (firmiSopstvenici == null)
            {
                return NotFound();
            }

            return Ok(firmiSopstvenici);
        }

        // PUT: api/FirmiSopstvenicisAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFirmiSopstvenici([FromRoute] int id, [FromBody] FirmiSopstvenici firmiSopstvenici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != firmiSopstvenici.Id)
            {
                return BadRequest();
            }

            _context.Entry(firmiSopstvenici).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FirmiSopstveniciExists(id))
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

        // POST: api/FirmiSopstvenicisAPI
        [HttpPost]
        public async Task<IActionResult> PostFirmiSopstvenici([FromBody] FirmiSopstvenici firmiSopstvenici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FirmiSopstvenici.Add(firmiSopstvenici);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFirmiSopstvenici", new { id = firmiSopstvenici.Id }, firmiSopstvenici);
        }

        // DELETE: api/FirmiSopstvenicisAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFirmiSopstvenici([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firmiSopstvenici = await _context.FirmiSopstvenici.FindAsync(id);
            if (firmiSopstvenici == null)
            {
                return NotFound();
            }

            _context.FirmiSopstvenici.Remove(firmiSopstvenici);
            await _context.SaveChangesAsync();

            return Ok(firmiSopstvenici);
        }

        private bool FirmiSopstveniciExists(int id)
        {
            return _context.FirmiSopstvenici.Any(e => e.Id == id);
        }
    }
}