using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        public IActionResult Authors()
        {
            return View();
        }
    }
}