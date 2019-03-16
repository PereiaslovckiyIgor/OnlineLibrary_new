using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Xml;

namespace LibOnline.Models.Authors
{
    public class Author
    {
        [Key]
        public int IdAuthor { get; set; }
        public string AuthorFullName { get; set; }

        public Author(int IdAuthor, string AuthorFullName)
        {
            this.IdAuthor = IdAuthor;
            this.AuthorFullName = AuthorFullName;
        }//Author
    }
}
