using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibOnline.Controllers.UserPage
{
    public class UserPageController : Controller
    {
        public IActionResult UserPage()
        {
            return View();
        }
    }
}