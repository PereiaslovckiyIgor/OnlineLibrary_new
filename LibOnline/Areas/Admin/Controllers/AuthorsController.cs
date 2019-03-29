using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Aythor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Authors()
        {
            return View();
        }

        // Получить списко всех авторов
        public IActionResult GetAuthors()
        {

            List<Author> authors = new List<Author>();

            using (ApplicationContext db = new ApplicationContext())
                authors = db.authors.OrderBy(a => a.AuthorFullName).ToList();

            return Json(authors);
        }//GetAuthors

        // Добовление автора
        public IActionResult AuthorInsert(string AuthorFullName) {

            string ResponseText;
            bool IsSuccess;

            Author author = new Author();

            // Проверка на наличие в БД
            using (ApplicationContext db = new ApplicationContext())
                author = db.authors.FirstOrDefault(a => a.AuthorFullName == AuthorFullName);

            if (author == null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.authors.Add(new Author(AuthorFullName, true));
                    db.SaveChanges();
                }
                ResponseText = "Автор успешно добавлен";
                IsSuccess = true;

            }
            else {
                ResponseText = "Автор уже есть в списке";
                IsSuccess = false;
            }
          

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//AuthorInsert
    }
}