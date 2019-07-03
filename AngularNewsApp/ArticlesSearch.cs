using AngularNewsApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularNewsApp
{
    public class ArticlesSearch
    {
        private readonly NewsAppContext _context;

        public ArticlesSearch()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsAppContext>();
            optionsBuilder.UseSqlServer("Server=192.168.1.198;Initial Catalog=newsapp; Persist security info=True; User Id=sa; Password=azertySQLServer!;");

            _context = new NewsAppContext(optionsBuilder.Options);
        }

        public void Search()
        {
            List<RssLink> links = _context.RssLink.ToList();

            List<Article> articles = new List<Article>();

            foreach (RssLink link in links)
            {
                List<Article> temp = new List<Article>();

                temp = RssFluxHelper.GetArticlesFromRssFlux(link.link);

                temp.ForEach(x => { x.id_rss_link = link.id; });

                articles.AddRange(temp);
            }

            articles = articles.Where(x => !_context.Article.Select(a => a.title).Contains(x.title)).ToList();

            _context.AddRange(articles);

            _context.SaveChanges();
        }
    }
}
