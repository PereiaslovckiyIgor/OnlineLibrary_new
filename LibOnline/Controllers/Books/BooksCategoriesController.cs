using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibOnline.Models;
using LibOnline.Models.BooksCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Controllers
{
    public class BooksCategoriesController : Controller
    {
        public IActionResult BooksCategories()
        {
            return View();
        }


        public IActionResult GetBooksCategories(int idCategory, int pageNumber)
        {
            List<BooksCatogoriesToShow> books = new List<BooksCatogoriesToShow>();
            List<BooksCategories> categories = new List<BooksCategories>();

            SqlParameter CategoryId = new SqlParameter("@IdCategory", idCategory);
            SqlParameter pNumber = new SqlParameter("@PageNumber", pageNumber);
           
            using (ApplicationContext db = new ApplicationContext())
                categories = db.booksCategories.FromSql($"EXECUTE [books].[GetBooksCategories] {CategoryId}, {pNumber}").ToList();

            categories.ForEach(item => books.Add(new BooksCatogoriesToShow(item)));
            ViewBag.booksCategories = books;

            return View("BooksCategories");
        }//GetBooksCategories
    }
}