using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibOnline.Controllers
{
    public class BooksCategoriesController : Controller
    {
        public IActionResult BooksCategories(int idCategory)
        {
            return View();
        }
    }
}