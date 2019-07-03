using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularNewsApp.Models
{
    public class ArticleUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int id_user { get; set; }
        public int id_article { get; set; }
        public User User { get; set; }
        public Article Article { get; set; }
    }
}
