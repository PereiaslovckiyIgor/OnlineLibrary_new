using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LibOnline.Models;
using LibOnline.Models.BooksCategories;
using LibOnline.Models.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Controllers
{
    public class BooksCategoriesController : Controller
    {

        const int pageElems = 24;

        public IActionResult BooksCategories()
        {
            return View();
        }


        # region BooksCategories жанры
        // Вывод Жанров
        public IActionResult GetBooksCategories(int idCategory, int pageNumber)
        {
            List<BooksCatogoriesToShow> books = new List<BooksCatogoriesToShow>();
            List<BooksCategories> categories = new List<BooksCategories>();
            List<CategoriesPagination> catPagination = new List<CategoriesPagination>();

            SqlParameter CategoryId = new SqlParameter("@IdCategory", idCategory);
            SqlParameter pNumber = new SqlParameter("@PageNumber", pageNumber);

            using (ApplicationContext db = new ApplicationContext())
                categories = db.booksCategories.FromSql($"EXECUTE [books].[GetBooksCategories] {CategoryId}, {pNumber}").ToList();

            categories.ForEach(item => books.Add(new BooksCatogoriesToShow(item)));

            using (ApplicationContext db = new ApplicationContext())
                catPagination = db.categoriesPagination.FromSql($"EXECUTE books.CategoriesPadination {CategoryId}, {pNumber}").ToList();

            ViewBag.booksCategories = books;
            ViewBag.pagination = catPagination[0];

            return View("BooksCategories");
        }//GetBooksCategories
        #endregion

        #region BooksCategories Популярное
        // Выбор популярных книг из меню
        public IActionResult GetPopularBooks(int pageNumber)
        {



            List<BooksCategories> populars = new List<BooksCategories>();
            List<BooksCatogoriesToShow> books = new List<BooksCatogoriesToShow>();
            List<MenuPagination> menuPagination = new List<MenuPagination>();

            SqlParameter pNumber = new SqlParameter("@PageNumber", pageNumber);
            SqlParameter rowPage = new SqlParameter("@RowspPage", pageElems);


            using (ApplicationContext db = new ApplicationContext())
                populars = db.booksCategories.FromSql($"EXECUTE [books].[GetPopularBooksByRating] {pNumber}, {rowPage}").ToList();

            populars.ForEach(item => books.Add(new BooksCatogoriesToShow(item)));


            using (ApplicationContext db = new ApplicationContext())
                menuPagination = db.menuPagination.FromSql($"EXECUTE books.MenuPadination {pageNumber}, {pageElems}").ToList();


            ViewBag.booksCategories = books;
            ViewBag.pagination = menuPagination[0];
            ViewBag.pagination.CategoryName = "Популярное";

            return View("BooksCategories");
        }//GetPopularBooks
        #endregion

        #region Новые книги за последние 2 года
        // Новые книги за последние 2 года
        public IActionResult GetNewBooks(int pageNumber)
        {
            List<BooksCategories> newBooks = new List<BooksCategories>();
            List<BooksCatogoriesToShow> books = new List<BooksCatogoriesToShow>();
            List<MenuPagination> menuPagination = new List<MenuPagination>();

            SqlParameter pNumber = new SqlParameter("@PageNumber", pageNumber);
            SqlParameter rowPage = new SqlParameter("@RowspPage", pageElems);


            using (ApplicationContext db = new ApplicationContext())
                newBooks = db.booksCategories.FromSql($"EXECUTE [books].[GetNewBooksByReleasedDate] {pNumber}, {rowPage}").ToList();

            newBooks.ForEach(item => books.Add(new BooksCatogoriesToShow(item)));


            using (ApplicationContext db = new ApplicationContext())
                menuPagination = db.menuPagination.FromSql($"EXECUTE books.MenuPadination {pageNumber}, {pageElems}").ToList();


            ViewBag.booksCategories = books;
            ViewBag.pagination = menuPagination[0];
            ViewBag.pagination.CategoryName = "Новое";

            return View("BooksCategories");
        }//GetNewBooks
        #endregion

    }
}