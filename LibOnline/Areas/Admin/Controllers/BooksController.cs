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
using System.Xml.Linq;

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


                    if (ParseBookOnPages(bookToInsert.BookPath))
                    {
                        ResponseText = "Книга успешно добавлена";
                        IsSuccess = true;
                    }
                    else
                    {
                        ResponseText = "Книга добавлена но страницы нет!";
                        IsSuccess = false;
                    }
                }
                catch
                {
                    ResponseText = "Ошибка на сервере";
                    IsSuccess = false;

                }//try-catch
            }
            else
            {
                ResponseText = "Эта книга уже есть в библиотеке";
                IsSuccess = false;
            }//if-else

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//AddNewBook

        // Парсинг  книги на старницы
        private bool ParseBookOnPages(string txtBookFileName)
        {
            List<Page> pages = new List<Page>();

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
                    sb.Append("&nbsp;" + item + "\r\n");


                pages.Add(new Page(i + 1, sb.ToString()));
                start += offset;


                if (start + offset > list.Count)
                    offset = list.Count - start;

            }//for

            bool resul = true; ;
            try
            {
                // Формируем 'XML' файл для пердачи в процедуру      
                XElement xEmp = new XElement("Pages",
                                    pages.Select(p =>
                                        new XElement("page",
                                         new XElement("PageText", p.PageText),
                                         new XElement("PageNumber", p.PageNumber))));

                string strss = xEmp.ToString();

                SqlParameter x_Emp = new SqlParameter("@xEmp", strss);

                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"[admin].[InsertPages] {x_Emp}");



            }
            catch (Exception ex)
            {
                //string e = ex.Message;
                resul = false;
            }
            return resul;
        }//ParseBookOnPages

        // Проверка на наличие книги при добовлении
        private bool IsBookNotExists(string bookNmae)
        {

            bool isExists;

            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.bookMains.Any(b => b.BookName == bookNmae && b.IsActive == true))
                    isExists = false;
                else
                    isExists = true;
            }
            return isExists;
        }//IsBookNotExists


        public IActionResult GetBookTextToUpdate(int IdBook)
        {
            BookMain book = new BookMain();
            using (ApplicationContext db = new ApplicationContext())
                book = db.bookMains.FirstOrDefault(b => b.IdBook == IdBook);

            return Json(new { bookName = book.BookName, booksDescription = book.BooksDescription });
        }//GetBookTextToUpdate

        [HttpPost]
        public IActionResult UpdateBookText(int IdBook, string BookName, string BookDescription)
        {
            string ResponseText;
            bool IsSuccess;

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    BookMain bookMain = db.bookMains.FirstOrDefault(b => b.IdBook == IdBook);

                    if (bookMain != null)
                    {
                        bookMain.BookName = BookName;
                        bookMain.BooksDescription = BookDescription;
                        db.SaveChanges();
                    }


                    ResponseText = "Описание книги изменено";
                    IsSuccess = true;
                }//using
            }
            catch
            {
                ResponseText = "Описание книги не изменено";
                IsSuccess = true;
            }
            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//UpdateBookText

        // Получить картинку для изменения
        public IActionResult GetImageToUpdate(int IdBook)
        {
            Image image = new Image();
            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);

            using (ApplicationContext db = new ApplicationContext())
                image = db.images.FromSql($"EXECUTE [admin].GetImagetoUpdate {idBook}").FirstOrDefault();

            return Json(new { idImage = image.IdImage, imagePath = image.ImagePath.Split('/').Last()});
        }//GetImageToUpdate

        // Обновить  Изображение книги
        public IActionResult BookImageUpdate(string ImgName, int IdBook) {

            string ResponseText;
            bool IsSuccess;


            SqlParameter imgName = new SqlParameter("@ImgName", "~/LibData/img/"+ImgName);
            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"EXECUTE [admin].BooktImageUpdate {imgName}, {idBook}");

                ResponseText = "Изображение книги изменено";
                IsSuccess = true;
            }
            catch {
                ResponseText = "Изображение книги не изменено";
                IsSuccess = false;

            }//try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//BookImageUpdate

        // Обновить дату
        public IActionResult ReleasedDataUpdate(int IdBook, DateTime ReleasedData)
        {
            string ResponseText;
            bool IsSuccess;

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    BookMain bookMain = db.bookMains.FirstOrDefault(b => b.IdBook == IdBook);

                    if (bookMain != null)
                    {
                        bookMain.ReleasedData = ReleasedData;
                        db.SaveChanges();
                    }


                    ResponseText = "Дата изменена";
                    IsSuccess = true;
                }//using
            }
            catch
            {
                ResponseText = "Дата не изменена";
                IsSuccess = true;
            }
            return Json(new { success = IsSuccess, responseText = ResponseText });


        }//ReleasedDataUpdate

        // Получить всех автров у книги
        public IActionResult GetAuthorsToUpdate(int IdBook)
        {
            List<BooksAuthors> bAuthors = new List<BooksAuthors>();
            List<Author> authors = GetAuthors();

            using (ApplicationContext db = new ApplicationContext())
                bAuthors = db.booksAuthors.Where(ba => ba.IdBook == IdBook).ToList();


            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in authors)
            {
                if (bAuthors.Find(ba => ba.IdAuthor == item.IdAuthor) != null)
                    stringBuilder.Append("<option selected value="+ item.IdAuthor + ">" + item.AuthorFullName + "</option>");
                else
                    stringBuilder.Append("<option value=" + item.IdAuthor + ">" + item.AuthorFullName + "</option>");
            }


            return Json(stringBuilder.ToString());
        }//GetAuthorsToUpdate

        // Обновить авторов у книги
        public IActionResult BookAuthorsUpdate(int IdBook, string Authors)
        {
            string ResponseText;
            bool IsSuccess;


            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);
            SqlParameter authors = new SqlParameter("@Authors", Authors);

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"EXECUTE[admin].BookAuthorsUpdate {idBook}, {authors}");

                ResponseText = "Авторы изменены";
                IsSuccess = true;
            }
            catch
            {
                ResponseText = "Авторы не изменены";
                IsSuccess = false;

            }//try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//Authors

        // Получить все жанры у книги
        public IActionResult GetCategoriesToUpdate(int IdBook)
        {
            List<BookCategories> bCategories = new List<BookCategories>();
            List<Category> categories = GetСategories();

            using (ApplicationContext db = new ApplicationContext())
                bCategories = db.booksCategories.Where(bc => bc.IdBook == IdBook).ToList();


            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in categories)
            {
                if (bCategories.Find(ba => ba.IdCategory == item.IdCategory) != null)
                    stringBuilder.Append("<option selected value=" + item.IdCategory + ">" + item.CategoryName + "</option>");
                else
                    stringBuilder.Append("<option value=" + item.IdCategory + ">" + item.CategoryName + "</option>");
            }

            return Json(stringBuilder.ToString());
        }//GetAuthorsToUpdate

        // Обновить жанры у книги
        public IActionResult BookCategoriesUpdate(int IdBook, string Categories)
        {
            string ResponseText;
            bool IsSuccess;


            SqlParameter idBook = new SqlParameter("@IdBook", IdBook);
            SqlParameter categories = new SqlParameter("@Categories", Categories);

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                    db.Database.ExecuteSqlCommand($"EXECUTE[admin].BookCategoriesUpdate {idBook}, {categories}");

                ResponseText = "Жанры изменены";
                IsSuccess = true;
            }
            catch
            {
                ResponseText = "Жанры не изменены";
                IsSuccess = false;

            }//try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//Authors

        // Заблокировать / Активировать книгу
        public IActionResult IsActiveToUpdate(int IdBook, bool IsActive)
        {
            string ResponseText;
            bool IsSuccess;

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    BookMain bookMain = db.bookMains.FirstOrDefault(b => b.IdBook == IdBook);

                    if (bookMain != null)
                    {
                        bookMain.IsActive = IsActive;
                        db.SaveChanges();
                    }


                    ResponseText = "Активность изменена";
                    IsSuccess = true;
                }//using
            }
            catch
            {
                ResponseText = "Активность не изменена";
                IsSuccess = true;
            }
            return Json(new { success = IsSuccess, responseText = ResponseText });



        }//IsActiveToUpdate

    }//BooksController
}//namespace