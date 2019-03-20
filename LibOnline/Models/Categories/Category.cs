using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.Categories
{
    public class Category
    {
        // Поля для обьекта из процедуры
        [Key]
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }

        public Category(int IdCategory, string CategoryName)
        {
            this.IdCategory = IdCategory;
            this.CategoryName = CategoryName;
        }//c-tor
    }//Category
}//CategoryImage
