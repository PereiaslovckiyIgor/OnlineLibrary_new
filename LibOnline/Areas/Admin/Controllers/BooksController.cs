using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Book;
using Microsoft.EntityFrameworkCore;
using LibOnline.Areas.Admin.Models.Aythor;
using LibOnline.Areas.Admin.Models.Category;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Books()
        {
            ViewBag.authors = GetAuthors();
            ViewBag.categories = GetСategories();

            return View();
        }//Books

        // Получит список книг для главной таблицы
        [Authorize(Roles = "Admin")]
        public IActionResult GetBooks()
        {
            List<Book> books = new List<Book>();

            using (ApplicationContext db = new ApplicationContext())
                books = db.books.FromSql("EXECUTE admin.GetAllBooks").ToList();

            return Json(books);
        }//GetAction

        // Все авторы
        private List<Author> GetAuthors() {
            List<Author> authors = new List<Author>();

            using (ApplicationContext db = new ApplicationContext())
                authors = db.authors.Where(a => a.IsActive == true)
                                           .OrderBy(a => a.AuthorFullName)
                                           .ToList();
             return authors;
        }//GetAuthors


        // Все Жанры
        private List<Category>GetСategories()
        {
            List<Category> categories = new List<Category>();

            using (ApplicationContext db = new ApplicationContext())
                categories = db.сategories.Where(c => c.IsActive == true)
                                           .OrderBy(c => c.CategoryName)
                                           .ToList();
            return categories;
        }//GetAuthors

    }//BooksController
}//namespace