using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LibOnline.Models;
using LibOnline.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Controllers.UserPage
{
    public class UserPageController : Controller
    {

        public IActionResult UserPage()
        {
            string UserName = "";

            // ПОЛУЧИТЬ ИМЯ И РОЛЬ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }//if

            List<UserBook> userBooks = new List<UserBook>();
            List<UserBookToShow> userBooksToShow = new List<UserBookToShow>(); 

            SqlParameter userName = new SqlParameter("@UserName", UserName);

            using (ApplicationContext db = new ApplicationContext())
                userBooks = db.userBooks.FromSql($"EXECUTE [books].[GetAllUSerBooks] {UserName}").ToList();

            userBooks.ForEach(item => userBooksToShow.Add(new UserBookToShow(item)));
            ViewBag.UserBooks = userBooksToShow;

            return View("UserPage");
        }//UserPage

    }
}