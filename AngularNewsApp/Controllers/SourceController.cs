using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularNewsApp.Models;

namespace AngularNewsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly NewsAppContext _context;

        public SourceController(NewsAppContext context)
        {
            _context = context;
        }

        // GET: api/Source
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Source>>> GetSource()
        {
            return await _context.Source.ToListAsync();
        }

        // GET: api/Source/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Source>> GetSource(int id)
        {
            var source = await _context.Source.FindAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            return source;
        }

        // PUT: api/Source/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSource(int id, Source source)
        {
            if (id != source.id)
            {
                return BadRequest();
            }

            _context.Entry(source).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceExists(id))
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

        // POST: api/Source
        [HttpPost]
        public async Task<ActionResult<Source>> PostSource(Source source)
        {
            _context.Source.Add(source);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSource", new { id = source.id }, source);
        }

        // DELETE: api/Source/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Source>> DeleteSource(int id)
        {
            var source = await _context.Source.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }

            _context.Source.Remove(source);
            await _context.SaveChangesAsync();

            return source;
        }

        private bool SourceExists(int id)
        {
            return _context.Source.Any(e => e.id == id);
        }
    }
}
