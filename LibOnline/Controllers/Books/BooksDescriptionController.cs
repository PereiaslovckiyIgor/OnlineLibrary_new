using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Models.Books;
using LibOnline.Models;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

    }//BooksDescriptionController
}//Books Namespace