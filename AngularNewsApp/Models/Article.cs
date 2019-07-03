using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AngularNewsApp.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public DateTime? date { get; set; }
        public int id_rss_link { get; set; }

        public RssLink Link { get; set; }
        public List<ArticleUser> ArticleUsers { get; set; }
    }
    
}
