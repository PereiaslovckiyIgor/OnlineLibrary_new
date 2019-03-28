using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Books()
        {
            return View();
        }
    }
}