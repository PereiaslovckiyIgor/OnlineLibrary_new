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
using LibOnline.Models.General;

namespace LibOnline.Controllers.UserPage
{
    public class UserPageController : Controller
    {

        public IActionResult UserPage(int pageNumber)
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
            SqlParameter pNumber = new SqlParameter("@PageNumber", pageNumber);

            // Основной запрос
            using (ApplicationContext db = new ApplicationContext())
                userBooks = db.userBooks.FromSql($"EXECUTE [books].[GetAllUSerBooks] {UserName}, {pNumber}").ToList();

            userBooks.ForEach(item => userBooksToShow.Add(new UserBookToShow(item)));



            // Пагинация
            List<PagePagination> userPagePagination = new List<PagePagination>();

            using (ApplicationContext db = new ApplicationContext())
                userPagePagination = db.pagePaginations.FromSql($"EXECUTE [books].[UserPagePadination] {pNumber}, {UserName}").ToList();


            ViewBag.UserBooks = userBooksToShow;
            ViewBag.userPagePagination = userPagePagination[0];
            return View("UserPage");
        }//UserPage

    }
}