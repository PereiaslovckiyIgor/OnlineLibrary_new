
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Access;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            List<Role> role = new List<Role>();

            using (ApplicationContext db = new ApplicationContext())
                role = db.roles.OrderByDescending(r => r.Id).ToList();

            return View(role);
        }//Users

        [Authorize(Roles = "Admin")]
        // Получить всех пользователей и их роли
        public IActionResult GetUsersAndRoles()
        {

            List<User> users = new List<User>();

            using (ApplicationContext db = new ApplicationContext())
                users = db.users.FromSql("EXECUTE [admin].[GetUsersAndRoles]").ToList();

                return Json(users);            
        }//GetUsersAndRoles



    }//UsersController
}//namespace