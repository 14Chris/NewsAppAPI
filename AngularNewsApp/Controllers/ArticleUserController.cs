using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularNewsApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace AngularNewsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleUserController : ControllerBase
    {
        private readonly NewsAppContext _context;

        public ArticleUserController(NewsAppContext context)
        {
            _context = context;
        }

        // GET: api/ArticleUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleUser>>> GetArticleUser()
        {
            return await _context.ArticleUser.ToListAsync();
        }

        // GET: api/ArticleUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleUser>> GetArticleUser(int id)
        {
            var articleUser = await _context.ArticleUser.FindAsync(id);

            if (articleUser == null)
            {
                return NotFound();
            }

            return articleUser;
        }

        // PUT: api/ArticleUser/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticleUser(int id, ArticleUser articleUser)
        {
            if (id != articleUser.id)
            {
                return BadRequest();
            }

            _context.Entry(articleUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleUserExists(id))
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

        // POST: api/ArticleUser
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArticleUser>> PostArticleUser(ArticleUser articleUser)
        {
            int userId = Convert.ToInt32(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value);

            bool alreadyExist = _context.ArticleUser.Where(x => x.id_user == userId && x.id_article == articleUser.id_article).Count() > 0;

            if (!alreadyExist)
            {
                articleUser.id_user = userId;

                _context.ArticleUser.Add(articleUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetArticleUser", new { id = articleUser.id }, articleUser);
            }

            return Ok();
        }

        // DELETE: api/ArticleUser/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticleUser>> DeleteArticleUser(int id)
        {
            var articleUser = await _context.ArticleUser.FindAsync(id);
            if (articleUser == null)
            {
                return NotFound();
            }

            _context.ArticleUser.Remove(articleUser);
            await _context.SaveChangesAsync();

            return articleUser;
        }

        private bool ArticleUserExists(int id)
        {
            return _context.ArticleUser.Any(e => e.id == id);
        }
    }
}
