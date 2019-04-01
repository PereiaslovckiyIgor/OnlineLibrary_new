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
}
