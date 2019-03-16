using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibOnline.Controllers.Books
{
    public class BooksPageController : Controller
    {
        public IActionResult BooksPage()
        {
            return View();
        }
    }
}