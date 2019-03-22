using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.General
{
    public class CategoriesPagination
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int PageNumber { get; set; }
        public int CountPages { get; set; }
    }//CategoriesPagination


    public class MenuPagination
    {
        [Key]
        public int PageNumber { get; set; }
        public int CountPages { get; set; }
        public string CategoryName { get; set; }
    }//CategoriesPagination

    public class PagePagination
    {
        [Key]
        public int PageNumber { get; set; }
        public int CountPages { get; set; }
    }//CategoriesPagination
}
