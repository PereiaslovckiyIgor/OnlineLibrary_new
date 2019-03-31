
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Access;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

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
        
        // Получить всех пользователей и их роли
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsersAndRoles()
        {

            List<User> users = new List<User>();

            using (ApplicationContext db = new ApplicationContext())
                users = db.users.FromSql("EXECUTE [admin].[GetUsersAndRoles]").ToList();

                return Json(users);            
        }//GetUsersAndRoles

        // Обновить Доступ пользователя И/ИЛИ его роль
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUserAndRole(int IdUser, int IdRole, bool IsActive)
        {
            string ResponseText;
            bool IsSuccess;

            try
            {
                SqlParameter idUser = new SqlParameter("@IdUser", IdUser);
                SqlParameter idRole = new SqlParameter("@IdRole", IdRole);
                SqlParameter isActive = new SqlParameter("@IsActive", IsActive);


                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Database.ExecuteSqlCommand($"admin.UpdateUserAndRole {IdUser}, {IdRole}, {IsActive}");

                    ResponseText = "Данные успешно изменены";
                    IsSuccess = true;
                }//using
            }
            catch {
                ResponseText = "Ошибка на сервере.";
                IsSuccess = false;
            }// tre-catch
            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//UpdateUserAndRole

    }//UsersController
}//namespace