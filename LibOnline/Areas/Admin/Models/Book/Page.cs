using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Book
{
    public class Page
    {
        public int PageNumber { get; set; }
        public string PageText { get; set; }

        public Page(int num, string txt)
        {
            PageNumber = num;
            PageText = txt;
        }
    }//Page
}
