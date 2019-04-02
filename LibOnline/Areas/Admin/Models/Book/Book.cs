using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Book
{
    [Table("Books", Schema = "books")]
    public class Book
    {
        [Key]
        public int IdBook  { get; set; }
        public string BookName  { get; set; }
        public string ReleasedData  { get; set; }
        public bool IsActive  { get; set; }
    }

    public class BookToInsert
    {
        [Key]
        public string BookName { get; set; }
        public DateTime ReleasedData { get; set; }
        public string BooksDescription { get; set; }
        public string ImagePath { get; set; }
        public string BookPath { get; set; }
        public string IdAuthor { get; set; }
        public string IdCategory { get; set; }
    }//BookToInsert

}
