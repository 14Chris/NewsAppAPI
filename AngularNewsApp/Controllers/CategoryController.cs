using AngularNewsApp.DataModels;
using AngularNewsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularNewsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly NewsAppContext _context;

        public CategoryController(NewsAppContext context)
        {
            _context = context;
        }

        // GET: api/NewsCategory
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDataModel>> GetNewsCategory()
        {
            return _context.Category.Select(x=>new CategoryDataModel() { id = x.id,libelle=x.libelle}).ToList();
        }

        // GET: api/NewsCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDataModel>> GetNewsCategory(int id)
        {
            var newsCategory = await _context.Category.FindAsync(id);

            if (newsCategory == null)
            {
                return NotFound();
            }

            CategoryDataModel cat = new CategoryDataModel() { id = newsCategory.id, libelle = newsCategory.libelle };

            return cat;
        }

    }
}
