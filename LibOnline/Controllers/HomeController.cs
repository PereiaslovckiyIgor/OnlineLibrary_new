using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Models;
using LibOnline.Models.Categories;
using Microsoft.EntityFrameworkCore;
using LibOnline.Models.BooksCategories;
using System.Data.SqlClient;

namespace LibOnline.Controllers
{
    public class HomeController : Controller
    {
        // Начальная инициализация страницы
        // Состоит из 3 частей: Популярние книги, Новые книги, Недавно добавленные книги
        public IActionResult Index()
        {
            ViewBag.popularBooks = getPopulars();
            ViewBag.newBooks = getNewBooks();

            return View();
        }//Index

        // Получит все жанры книг
        public JsonResult GetAllCategories()
        {
            List<AllCategories> list = new List<AllCategories>();
            using (ApplicationContext db = new ApplicationContext())
                list = db.allCategories.FromSql("EXECUTE [general].[GetAllCategories]").ToList();

            return Json(list);
        }//GetAllCategories


        //  Популярные книги 
        private List<BooksCatogoriesToShow> getPopulars() {

            List<BooksCategories> populars = new List<BooksCategories>();
            List<BooksCatogoriesToShow> books = new List<BooksCatogoriesToShow>();


            using (ApplicationContext db = new ApplicationContext())
                populars = db.booksCategories.FromSql("EXECUTE [books].[GetPopularBooksByRating]").ToList();

            populars.ForEach(item => books.Add(new BooksCatogoriesToShow(item)));

            return books;
        }//getPopulars


        // Новые книги по дате выпуска
        private List<BooksCatogoriesToShow> getNewBooks()
        {
            List<BooksCategories> newBooks = new List<BooksCategories>();
            List<BooksCatogoriesToShow> books = new List<BooksCatogoriesToShow>();

            using (ApplicationContext db = new ApplicationContext())
                newBooks = db.booksCategories.FromSql("EXECUTE [books].[GetNewBooksByReleasedDate]").ToList();

            newBooks.ForEach(item => books.Add(new BooksCatogoriesToShow(item)));

            return books;
        }//getNewBooks

        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
