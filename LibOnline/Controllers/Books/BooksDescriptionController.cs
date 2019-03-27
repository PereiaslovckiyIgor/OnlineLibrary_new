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

        #region получить описание книги и коментариев
        //GetBookDescription 
        public IActionResult GetBookDescription(int IdBook)
        {
            bool isAdmin = false;
            // ПОЛУЧИТЬ ИМЯ И РОЛЬ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                isAdmin = role == "Admin" ? true : false;
            }

            List<BookDescription> bookDescriptions = new List<BookDescription>();
            List<Comment> comments = new List<Comment>();

            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);


            using (ApplicationContext db = new ApplicationContext())
                bookDescriptions = db.bookDescriptions.FromSql($"EXECUTE [books].[GetBooksDescription] {idBook}").ToList();

            using (ApplicationContext db = new ApplicationContext())
                comments = db.comments.FromSql($"EXECUTE books.GetBooksComments {idBook}").ToList();


            ViewBag.booksDescription = new BookDescriptionToShow(bookDescriptions[0]);
            ViewBag.booksComments = comments;
            ViewBag.isAdmin = isAdmin;
            return View("BooksDescription");
        }//GetBookDescription
        #endregion

        #region Добавление книги в Избранное
        public IActionResult AddInUserBooks(int IdBook)
        {

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
            }
            else
            {
                try
                {
                    SqlParameter UserId = new SqlParameter("@UserId", idUser);
                    SqlParameter BookId = new SqlParameter("@IdBook", IdBook);

                    using (ApplicationContext db = new ApplicationContext())
                        db.Database.ExecuteSqlCommand($"books.UserBooksInsertNewBook {UserId}, {BookId}");

                    ResponseText = "Книга добавлена в избранное";
                    IsSuccess = true;
                }
                catch
                {
                    ResponseText = "Ошибка. Книга не добавлена";
                    IsSuccess = false;
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

        #region Отправить отзыв
        public IActionResult SendComment(int idBook, string textComment)
        {
            string UserName = "", ResponseText;
            bool IsSuccess;


            // ПОЛУЧИТЬ ИМЯ И РОЛЬ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }//if

            try
            {
                SqlParameter BookId = new SqlParameter("@IdBook", idBook);
                SqlParameter TextComment = new SqlParameter("@TextComment", textComment);

                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"books.CommentPublish {BookId}, {TextComment}, {UserName}");

                ResponseText = "Ваш отзыв опубликован";
                IsSuccess = true;
            }
            catch
            {
                ResponseText = "Ваш отзыв опубликован не был";
                IsSuccess = false;
            }// try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
            //return RedirectToAction("GetBookDescription", "BooksDescription", new { IdBook  = idBook });
        }//GetBookComments
        #endregion

        #region Удалить отзыв
        public IActionResult RemoveComment(int idComment)
        {
            string ResponseText;
            bool IsSuccess;

            try
            {
                SqlParameter IdComment = new SqlParameter("@IdComment", idComment);

                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"books.RemoveComment {IdComment}");

                ResponseText = "Отзыв был удален";
                IsSuccess = true;
            }
            catch
            {
                ResponseText = "Отзыв не был удален";
                IsSuccess = false;
            }// try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//GetBookComments
        #endregion


        public IActionResult SetBookRating(int IdBook, int UsreRating)
        {

            string UserName = "";
            bool IsSuccess;


            // ПОЛУЧИТЬ ИМЯ И РОЛЬ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }//if

            // Получит ID пользователя
            int idUser = new Utils().GetUserIdByUserName(UserName);

            try
            {
                SqlParameter IdUser = new SqlParameter("@TextComment", idUser);
                SqlParameter idBook = new SqlParameter("@IdBook", IdBook);
                SqlParameter usreRating = new SqlParameter("@RatigValue", UsreRating);

                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"books.SetRating  {IdUser}, {idBook}, {usreRating}");

                IsSuccess = true;
            }
            catch
            {
                IsSuccess = false;
            }// try-catch

            return Json(new { success = IsSuccess });
        }//SetBookRating
    }//BooksDescriptionController
}//Books Namespace