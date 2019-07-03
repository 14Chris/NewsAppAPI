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
    public class LinkController : ControllerBase
    {
        private readonly NewsAppContext _context;

        public LinkController(NewsAppContext context)
        {
            _context = context;
        }

        // GET: api/Link
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RssLink>>> GetRssLink()
        {
            return await _context.RssLink.ToListAsync();
        }

        // GET: api/Link/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RssLink>> GetRssLink(int id)
        {
            var rssLink = await _context.RssLink.FindAsync(id);

            if (rssLink == null)
            {
                return NotFound();
            }

            return rssLink;
        }

        // PUT: api/Link/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRssLink(int id, RssLink rssLink)
        {
            if (id != rssLink.id)
            {
                return BadRequest();
            }

            _context.Entry(rssLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RssLinkExists(id))
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

        // POST: api/Link
        [HttpPost]
        public async Task<ActionResult<RssLink>> PostRssLink(RssLink rssLink)
        {
            bool okLien = _context.RssLink.Where(x => x.id_source == rssLink.id_source && x.link == rssLink.link).Count() == 0;

            if (okLien)
            {
                _context.RssLink.Add(rssLink);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetRssLink", new { id = rssLink.id }, rssLink);
            }

            return BadRequest();
        }

        // DELETE: api/Link/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RssLink>> DeleteRssLink(int id)
        {
            var rssLink = await _context.RssLink.FindAsync(id);
            if (rssLink == null)
            {
                return NotFound();
            }

            _context.RssLink.Remove(rssLink);
            await _context.SaveChangesAsync();

            return rssLink;
        }

        private bool RssLinkExists(int id)
        {
            return _context.RssLink.Any(e => e.id == id);
        }

    }
}
