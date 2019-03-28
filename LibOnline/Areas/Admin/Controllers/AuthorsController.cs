using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Aythor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Authors()
        {
            return View();
        }

        public IActionResult GetAuthors() {

            List<Author> authors = new List<Author>();

            using (ApplicationContext db = new ApplicationContext())
                authors = db.authors.OrderBy(a => a.AuthorFullName).ToList();

           return Json(authors);
        }//GetAuthors

    }
}