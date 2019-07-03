using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularNewsApp.Models
{
    public class UserValidationToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int id_user { get; set; }

        public string token { get; set; }

        public DateTime date { get; set; }

        public User User { get; set; }
    }
}
