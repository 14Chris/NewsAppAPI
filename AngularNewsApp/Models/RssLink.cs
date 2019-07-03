using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularNewsApp.Models
{
    public class RssLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string name { get; set; }

        public string link { get; set; }

        public int? id_category { get; set; }

        public int? id_source { get; set; }

        public Category Category { get; set; }
        public Source Source { get; set; }
        public List<Article> Articles { get; set; }
    }
}
