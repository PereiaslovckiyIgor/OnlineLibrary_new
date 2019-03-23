using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Models.Books;
using LibOnline.Models;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LibOnline.Models.General;

namespace LibOnline.Controllers.Books
{
    public class BooksDescriptionController : Controller
    {
        public IActionResult BooksDescription()
        {
            return View();
        }//BooksDescription

        #region получить описание книги без коментариев
        //GetBookDescription 
        public IActionResult GetBookDescription(int IdBook)
        {
            List<BookDescription> bookDescriptions = new List<BookDescription>();

            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);

            using (ApplicationContext db = new ApplicationContext())
                bookDescriptions = db.bookDescriptions.FromSql($"EXECUTE [books].[GetBooksDescription] {idBook}").ToList();


            ViewBag.booksDescription = new BookDescriptionToShow(bookDescriptions[0]);

            return View("BooksDescription");
        }//GetBookDescription
        #endregion

        #region Добавление книги в Избранное
        public IActionResult AddInUserBooks(int IdBook) {

            string UserName = "", ResponseText;
            bool IsSuccess;

            // ПОЛУЧИТЬ ИМЯ И РОЛЬ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }//if

            // Получит ID пользователя
            int idUser = new Utils().GetUserIdByUserName(UserName);


            if (IsBookInUserBooks(IdBook, idUser))
            {
                ResponseText = "Книга уже есть в вашем списке";
                IsSuccess = false;
            }else {
                try
                {
                    SqlParameter UserId = new SqlParameter("@UserId", idUser);
                    SqlParameter BookId = new SqlParameter("@IdBook", IdBook);

                    using (ApplicationContext db = new ApplicationContext())
                        db.Database.ExecuteSqlCommand($"books.UserBooksInsertNewBook {UserId}, {BookId}");

                    ResponseText = "Книга добавлена в избранное";
                    IsSuccess = true;
                } catch {
                    ResponseText = "Ошибка. Книга не добавлена";
                    IsSuccess = true;
                }// try-catch
            }//if-else

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//AddInUserBooks
        #endregion

        #region Проверка на наличие уже добавленной книги
        // Проверка на наличие уже добавленной книги
        private bool IsBookInUserBooks(int IdBook, int idUser)
        {
            GetUsersBooks ub;
            using (ApplicationContext db = new ApplicationContext())
                ub = db.getUserBooks.FirstOrDefault(item => item.IdBook == IdBook && item.IdUser == idUser);

            return (ub != null) ? true : false;
        }//IsBookInUserBooks
        #endregion

    }//BooksDescriptionController
}//Books Namespace