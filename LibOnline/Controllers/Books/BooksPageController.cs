using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Models.Books;
using System.Data.SqlClient;
using LibOnline.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LibOnline.Models.General;

namespace LibOnline.Controllers.Books
{
    public class BooksPageController : Controller
    {
        public IActionResult BooksPage()
        {
            return View();
        }//BooksPage


        public IActionResult GetPageContent(int IdBook, int PageNumber)
        {
            string UserName = "";

            // ПОЛУЧИТЬ ИМЯ И РОЛЬ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                //string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }

            List<Page> pageContetnt = new List<Page>();

            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);
            SqlParameter pageNumber = new SqlParameter("@PageNumber", PageNumber);
            SqlParameter userName = new SqlParameter("@UserName", UserName);

            using (ApplicationContext db = new ApplicationContext())
                pageContetnt = db.booksPage.FromSql($"EXECUTE [books].[GetBookPage] {idBook}, {pageNumber}, {userName}").ToList();

            // Пагинация
            List<PagePagination> pagePagination = new List<PagePagination>();
            using (ApplicationContext db = new ApplicationContext())
                pagePagination = db.pagePaginations.FromSql($"EXECUTE [books].[PagePadination] {idBook}, {pageNumber}").ToList();

            ViewBag.Page = pageContetnt[0];
            ViewBag.pagePagination = pagePagination[0];

            return View("BooksPage");
        }//GetPageContent

        // Получить список шрифтов через AJAX
        public JsonResult GetAllFontSizesValues()
        {
            List<PageFontSizes> sizes = new List<PageFontSizes>();
            using (ApplicationContext db = new ApplicationContext())
                sizes = db.pageFontSizes.ToList();

            return Json(sizes);
        }//GetAllFontSizesValues


        // Сохранить настройки польователя AJAX
        public IActionResult SaveUserSettings(string fs_Value)
        {

            string UserName = "", ResponseText;
            bool IsSuccess;

            // ПОЛУЧИТЬ ИМЯ ПОЛЬЗОВАТЕЛЯ
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }//if

            SqlParameter userName = new SqlParameter("@UserName", UserName);
            SqlParameter fsValue = new SqlParameter("@fsValue", fs_Value);

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"books.SaveUsersSetting {userName}, {fsValue}");

                IsSuccess = true;
                ResponseText = "Настройки сохранены";
            }
            catch
            {

                IsSuccess = false;
                ResponseText = "Настройки не сохранены";
            } //try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//SaveUserSettings


        // Добавление пользователем закладки
        public IActionResult AddUserBookmark(int IdBook, int IdPage)
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
            SqlParameter idPage = new SqlParameter("@IdPage", 2);

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"books.AddUserBookmark {userName}, {idBook}, {idPage}");

                IsSuccess = true;
                ResponseText = "Закладка добавленна";
            }
            catch
            {
                IsSuccess = false;
                ResponseText = "Закладка не добавлена";
            } //try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//AddUserBookmark


    }//BooksPageController 
}//namespace