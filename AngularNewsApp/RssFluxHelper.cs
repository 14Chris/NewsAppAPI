using AngularNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace AngularNewsApp
{
    public class RssFluxHelper
    {
        public static List<Article> GetArticlesFromRssFlux(string rssLink)
        {

            List<Article> articles = new List<Article>();

            var webClient = new WebClient();

            string result = webClient.DownloadString(rssLink);

            XDocument document = XDocument.Parse(result);

            articles = (from descendant in document.Descendants("item")
                        select new Article()
                        {
                            title = (descendant.Element("title")!=null) ? descendant.Element("title").Value : "",
                            description = (descendant.Element("description") != null) ? descendant.Element("description").Value : "",
                            link = (descendant.Element("link") != null) ? descendant.Element("link").Value.Replace("\n", string.Empty).Trim() : "",
                            date = (descendant.Element("pubDate") != null) ? Convert.ToDateTime(descendant.Element("pubDate").Value)/*.ToString("g", CultureInfo.CreateSpecificCulture("fr-FR"))*/ : (DateTime?)null
                        }).ToList();

            List<Article> articlesWithoutDate = articles.Where(x => x.date == null).ToList();

            if (articlesWithoutDate.Count() > 0)
            {
                DateTime? fileDate = (document.Descendants("channel").Select(x=>x.Element("lastBuildDate")).FirstOrDefault() != null) ? Convert.ToDateTime(document.Descendants("channel").Select(x => x.Element("lastBuildDate")).FirstOrDefault().Value)/*.ToString("g", CultureInfo.CreateSpecificCulture("fr-FR")*/ : (DateTime?)null;

                foreach (Article a in articlesWithoutDate)
                {
                    a.date = fileDate;
                }
            }

            return articles;
        }
    }
}
