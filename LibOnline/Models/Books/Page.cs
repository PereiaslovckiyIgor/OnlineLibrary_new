using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.Books
{

    public class Page
    {
        [Key]
        public int IdPage { get; set; }
        public int IdBook { get; set; }
        public string PageContent { get; set; }
        public int PageNumber { get; set; }
        public string TextFontSize { get; set; }
        public int BookPageCount { get; set; }
    }//Page
}
