using AngularNewsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunPythonScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AngularNewsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {

        private readonly NewsAppContext _context;

        public ArticleController(NewsAppContext context)
        {
            _context = context;

        }

        // GET: api/Article
        [HttpGet("{nb}")]
        public ActionResult<IEnumerable<ArticleDataModel>> Get(int? nb)
        {
            List<Article> articles = new List<Article>();

            if (nb == null)
            {
                articles = _context.Article
                             .OrderByDescending(x => x.date)
                             .ToList();
            }
            else
            {
                articles = _context.Article
                             .OrderByDescending(x => x.date)
                             .Take(nb.Value)
                             .ToList();
            }



            List<ArticleDataModel> art = (from a in articles
                                          from c in _context.Category
                                          from l in _context.RssLink
                                          from s in _context.Source
                                          where s.id == l.id_source
                                          where l.id_category == c.id
                                          where a.id_rss_link == l.id
                                          select new ArticleDataModel()
                                          {
                                              id = a.id,
                                              date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                                              description = a.description,
                                              link = a.link,
                                              source = s.name,
                                              subtitle = a.subtitle,
                                              title = a.title

                                          }).ToList();




            foreach (ArticleDataModel a in art)
            {
                Regex pRegex = new Regex("<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
                // if text is not single line use this regex
                // Regex pRegex = new Regex("<p.*?(?=</p>), RegexOptions.SingleLine"); 

                var result = pRegex.Match(a.description).Groups[1].Value;

                if (!string.IsNullOrEmpty(result))
                {
                    a.img = result;
                }
            }



            return art;
        }

        // GET: api/Article
        [HttpGet("category/{id}/{nb?}")]
        public ActionResult<IEnumerable<ArticleDataModel>> GetArticlesFromCategory(int id, int? nb)
        {
            List<Article> articles = new List<Article>();

            if (nb == null)
            {
                articles = _context.Article.Where(x => x.Link.id_category == id)
                           .OrderByDescending(x => x.date)
                           .ToList();
            }
            else
            {

                articles = _context.Article.Where(x => x.Link.id_category == id)
                            .OrderByDescending(x => x.date)
                            .Take(nb.Value)
                            .ToList();
            }

            List<ArticleDataModel> art = (from a in articles
                                          from c in _context.Category
                                          from l in _context.RssLink
                                          from s in _context.Source
                                          where s.id == l.id_source
                                          where l.id_category == c.id
                                          where a.id_rss_link == l.id
                                          select new ArticleDataModel()
                                          {
                                              id = a.id,
                                              date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                                              description = a.description,
                                              link = a.link,
                                              source = s.name,
                                              subtitle = a.subtitle,
                                              title = a.title

                                          }).ToList();


            foreach (ArticleDataModel a in art)
            {
                Regex pRegex = new Regex("<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
                // if text is not single line use this regex
                // Regex pRegex = new Regex("<p.*?(?=</p>), RegexOptions.SingleLine"); 

                var result = pRegex.Match(a.description).Groups[1].Value;

                if (!string.IsNullOrEmpty(result))
                {
                    a.img = result;
                }

            }

            return art;
        }


        // GET: api/Article
        [HttpGet("search/{search}/{nb}")]
        public ActionResult<IEnumerable<ArticleDataModel>> GetArticlesFromSearch(string search, int? nb)
        {
            List<Article> articles = new List<Article>();


            if (!string.IsNullOrEmpty(search))
            {

                if (nb == null)
                    articles = _context.Article.Where(x => x.title.Contains(search) || x.description.Contains(search)).ToList();
                else
                    articles = _context.Article.Where(x => x.title.Contains(search) || x.description.Contains(search)).Take(nb.Value).ToList();
            }
            else
            {
                if (nb == null)
                    articles = _context.Article.ToList();
                else
                    articles = _context.Article.Take(nb.Value).ToList();
            }

            List<ArticleDataModel> art = (from a in articles
                                          from c in _context.Category
                                          from l in _context.RssLink
                                          from s in _context.Source
                                          where s.id == l.id_source
                                          where l.id_category == c.id
                                          where a.id_rss_link == l.id
                                          select new ArticleDataModel()
                                          {
                                              id = a.id,
                                              date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                                              description = a.description,
                                              link = a.link,
                                              source = s.name,
                                              subtitle = a.subtitle,
                                              title = a.title
                                          }).ToList();


            foreach (ArticleDataModel a in art)
            {
                Regex pRegex = new Regex("<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
                // if text is not single line use this regex
                // Regex pRegex = new Regex("<p.*?(?=</p>), RegexOptions.SingleLine"); 

                var result = pRegex.Match(a.description).Groups[1].Value;

                if (!string.IsNullOrEmpty(result))
                {
                    a.img = result;
                }

            }

            return art;
        }

        // GET: Article
        [HttpGet("favorite/{nb}")]
        [Authorize]
        public ActionResult<IEnumerable<ArticleDataModel>> GetFavoriteArticlesFromUser(int? nb)
        {
            List<Article> articles = new List<Article>();

            int userId = Convert.ToInt32(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value);

            List<ArticleDataModel> art = new List<ArticleDataModel>();
            if (nb != null)
            {

                art = (from a in _context.Article
                       from au in _context.ArticleUser
                       where au.id_user == userId
                       where au.id_article == a.id
                       from l in _context.RssLink
                       from s in _context.Source
                       where s.id == l.id_source
                       where a.id_rss_link == l.id
                       select new ArticleDataModel()
                       {
                           id = a.id,
                           date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                           description = a.description,
                           link = a.link,
                           source = s.name,
                           subtitle = a.subtitle,
                           title = a.title
                       }).Take(nb.Value).ToList();
            }
            else
            {
                art = (from a in _context.Article
                       from au in _context.ArticleUser
                       where au.id_user == userId
                       where au.id_article == a.id
                       from l in _context.RssLink
                       from s in _context.Source
                       where s.id == l.id_source
                       where a.id_rss_link == l.id
                       select new ArticleDataModel()
                       {
                           id = a.id,
                           date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                           description = a.description,
                           link = a.link,
                           source = s.name,
                           subtitle = a.subtitle,
                           title = a.title
                       }).ToList();
            }

            foreach (ArticleDataModel a in art)
            {
                Regex pRegex = new Regex("<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
                // if text is not single line use this regex
                // Regex pRegex = new Regex("<p.*?(?=</p>), RegexOptions.SingleLine"); 

                var result = pRegex.Match(a.description).Groups[1].Value;

                if (!string.IsNullOrEmpty(result))
                {
                    a.img = result;
                }

            }


            return art;
        }

        // GET: Article
        [HttpGet("recommend/{nb?}")]
        [Authorize]
        public ActionResult<IEnumerable<ArticleDataModel>> GetArticlesRecommendationForUser(int? nb)
        {
            List<Article> articles = new List<Article>();

            int idUser = Convert.ToInt32(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value);

            List<ArticleDataModel> art = new List<ArticleDataModel>();

            RecommenderSystem p = new RecommenderSystem();

            List<int> recommendations = p.GetRecommendations(idUser);

            if (nb.HasValue)
            {
                art = (from a in _context.Article
                       from l in _context.RssLink
                       from s in _context.Source
                       where recommendations.Contains(a.id)
                       where s.id == l.id_source
                       where a.id_rss_link == l.id
                       select new ArticleDataModel()
                       {
                           id = a.id,
                           date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                           description = a.description,
                           link = a.link,
                           source = s.name,
                           subtitle = a.subtitle,
                           title = a.title
                       }).Take(nb.Value).ToList();
            }
            else
            {
                art = (from a in _context.Article
                       from l in _context.RssLink
                       from s in _context.Source
                       where recommendations.Contains(a.id)
                       where s.id == l.id_source
                       where a.id_rss_link == l.id
                       select new ArticleDataModel()
                       {
                           id = a.id,
                           date = (a.date.HasValue) ? a.date.Value.ToLongDateString() : "",
                           description = a.description,
                           link = a.link,
                           source = s.name,
                           subtitle = a.subtitle,
                           title = a.title
                       }).ToList();
            }


            foreach (ArticleDataModel a in art)
            {
                Regex pRegex = new Regex("<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase);
                // if text is not single line use this regex
                // Regex pRegex = new Regex("<p.*?(?=</p>), RegexOptions.SingleLine"); 

                var result = pRegex.Match(a.description).Groups[1].Value;

                if (!string.IsNullOrEmpty(result))
                {
                    a.img = result;
                }

            }
            return art;
        }
    }
}

