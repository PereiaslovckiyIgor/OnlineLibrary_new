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

            // ПОЛУЧИТЬ РОЛЬ ПОЛЬЗОВАТЕЛЯ
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


            ViewBag.Page = pageContetnt[0];
            ViewData["FonSizesArray"] = GetAllFontSizesValues();

            return View("BooksPage");
        }//GetPageContent


        public JsonResult GetAllFontSizesValues()
        {
            List<PageFontSizes> sizes = new List<PageFontSizes>();
            using (ApplicationContext db = new ApplicationContext())
                sizes = db.pageFontSizes.ToList();

            return Json(sizes);
        }//GetAllFontSizesValues
    }
}