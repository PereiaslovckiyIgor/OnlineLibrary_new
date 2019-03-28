using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using LibOnline.Models;
using LibOnline.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibOnline.Models.General;
using Microsoft.AspNetCore.Authorization;

namespace LibOnline.Controllers.UserPage
{
    public class UserPageController : Controller
    {
        #region Получить список Избраного
        [Authorize]
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
        #endregion

        #region Удалить книгу из Избраного
        // Удалить книгу из Избранного
        [Authorize]
        public IActionResult RemoveUserBook(int IdBook)
        {
            string UserName = "", ResponseText;
            bool IsSuccess;

            // ПОЛУЧИТЬ ИМЯ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }//if

            SqlParameter userName = new SqlParameter("@UserName", UserName);
            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"books.RemoveUserBook {userName}, {idBook}");

                IsSuccess = true;
                ResponseText = "Книга удалена из избранного";
            }
            catch
            {
                IsSuccess = false;
                ResponseText = "Книга не была удалена из избранного";
            } //try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });

        }//RemoveUserBook
        #endregion
    }//UserPageController
}//Namespace