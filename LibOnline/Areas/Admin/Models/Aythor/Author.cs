using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Aythor
{
    [Table("Authors", Schema = "general")]
    public class Author
    {
        [Key]
        public int IdAuthor { get; set; }
        public string AuthorFullName { get; set; }
        public bool IsActive { get; set; }
    }//Author
}
