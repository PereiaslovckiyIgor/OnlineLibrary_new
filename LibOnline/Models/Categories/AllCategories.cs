using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.Categories
{
    public class AllCategories
    {
        // Поля для обьекта из процедуры
        [Key]
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
    }//CategoryImage
}//CategoryImage
