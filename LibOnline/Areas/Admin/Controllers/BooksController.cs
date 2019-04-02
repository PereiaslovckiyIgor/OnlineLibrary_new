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
using System.Text;
using System.IO;
using System.Data.SqlClient;

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
        private List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();

            using (ApplicationContext db = new ApplicationContext())
                authors = db.authors.Where(a => a.IsActive == true)
                                           .OrderBy(a => a.AuthorFullName)
                                           .ToList();
            return authors;
        }//GetAuthors


        // Все Жанры
        private List<Category> GetСategories()
        {
            List<Category> categories = new List<Category>();

            using (ApplicationContext db = new ApplicationContext())
                categories = db.сategories.Where(c => c.IsActive == true)
                                           .OrderBy(c => c.CategoryName)
                                           .ToList();
            return categories;
        }//GetAuthors


        // Добовление новой книги
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewBook(BookToInsert bookToInsert)
        {
            string ResponseText;
            bool IsSuccess;

            // Проверка на наличие в БД
            if (IsBookNotExists(bookToInsert.BookName))
            {
                try
                {
                    SqlParameter bookName = new SqlParameter("@BookName", bookToInsert.BookName);
                    SqlParameter releasedData = new SqlParameter("@ReleasedData", bookToInsert.ReleasedData);
                    SqlParameter booksDescription = new SqlParameter("@BooksDescription", bookToInsert.BooksDescription);
                    SqlParameter imagePath = new SqlParameter("@ImagePath", "~/LibData/img/" + bookToInsert.ImagePath);
                    SqlParameter bookPath = new SqlParameter("@BookPath", bookToInsert.BookPath);
                    SqlParameter idAuthor = new SqlParameter("@IdAuthor", bookToInsert.IdAuthor);
                    SqlParameter idCategory = new SqlParameter("@IdCategory", bookToInsert.IdCategory);

                    using (ApplicationContext db = new ApplicationContext())
                        db.Database
                          .ExecuteSqlCommand($"[admin].[AddBook] {bookName}, {releasedData},{booksDescription},{imagePath},{bookPath},{idAuthor},{idCategory}");



                    ResponseText = "Книга успешно добавлена";
                    IsSuccess = true;
                }
                catch {
                    ResponseText = "Ошибка на сервере";
                    IsSuccess = false;

                }//try-catch
            }else {
                ResponseText = "Эта книга уже есть в библиотеке";
                IsSuccess = false;
            }//if-else

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//AddNewBook


        // Парсинг  книги на старницы
        private List<string> ParseBookOnPages(string txtBookFileName)
        {
            List<string> pages = new List<string>();

            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\LibData\txt\"}";
            string[] rows = System.IO.File.ReadAllLines(path + txtBookFileName);
            List<string> list = new List<string>(rows);



            int start = 0, offset = 50;

            int pagesCount = Convert.ToInt32(Math.Ceiling(rows.Length / (double)offset));


            for (int i = 0; i < pagesCount; i++)
            {
                StringBuilder sb = new StringBuilder();
                List<string> tmp = new List<string>();

                tmp.AddRange(list.GetRange(start, offset));

                foreach (var item in tmp)
                    sb.Append("\t"+ item+"\r\n");


                pages.Add(sb.ToString());
                start += offset;


                if (start + offset > list.Count)
                    offset = list.Count - start;

            }//for
            
            
            
            /*
            
            И создать класс Page

            XElement xEmp =
                            new XElement("Pages",
                                str.Select(p =>
                                    new XElement("page",
                                        new XElement("FirstName", p.number),
                                        new XElement("LastName", p.value))));
             
             
             
             */

            return pages;
        }//ParseBookOnPages

        // Проверка на наличие книги при добовлении
        private bool IsBookNotExists(string bookNmae)
        {

            Book book = new Book();

            using (ApplicationContext db = new ApplicationContext())
                book = db.books.FirstOrDefault(b => b.BookName == bookNmae && b.IsActive == true);



            return book == null ? true : false;
        }//IsBookNotExists

    }//BooksController
}//namespace