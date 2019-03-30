using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Category
{
    [Table("Categories", Schema = "general")]
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }

        public Category()
        {

        }

        public Category(string categoryName, bool isActive)
        {
            CategoryName = categoryName;
            IsActive = isActive;
        }
    }//Author
}
