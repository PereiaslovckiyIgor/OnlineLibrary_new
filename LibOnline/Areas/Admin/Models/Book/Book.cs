using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Book
{

    public class Book
    {
        [Key]
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string ReleasedData { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("Books", Schema = "books")]
    public class BookMain
    {
        [Key]
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public DateTime ReleasedData { get; set; }
        public string BooksDescription { get; set; }
        public bool IsActive { get; set; }
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


    public class BookToUpdate
    {
        [Key]
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public DateTime ReleasedData { get; set; }
        public string BooksDescription { get; set; }
        public string ImagePath { get; set; }
        public string IdAuthor { get; set; }
        public string IdCategory { get; set; }
    }//BookToInsert


    public class BookText {
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string BookDescription { get; set; }
    }//BookText

    public class Image {
        [Key]
        public int IdImage { get; set; }
        public string ImagePath { get; set; }
    }

    [Table("BooksAuthors", Schema = "books")]
    public class BooksAuthors
    {
        [Key]
        public int IdBookAuthor { get; set; }
        public int IdBook { get; set; }
        public int IdAuthor { get; set; }
    }

    [Table("BookCategories", Schema = "books")]
    public class BookCategories
    {
        [Key]
        public int IdBookCategories { get; set; }
        public int IdBook { get; set; }
        public int IdCategory { get; set; }
    }


}
