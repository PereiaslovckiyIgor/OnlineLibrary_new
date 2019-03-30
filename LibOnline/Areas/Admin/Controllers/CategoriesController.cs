using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Category;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {

        [Authorize(Roles = "Admin")]
        public IActionResult Categories()
        {
            return View();
        }

        // Получить списко всех авторов
        [Authorize(Roles = "Admin")]
        public IActionResult GetCategories()
        {

            List<Category> categories = new List<Category>();

            using (ApplicationContext db = new ApplicationContext())
                categories = db.сategories.OrderByDescending(a => a.IdCategory).ToList();

            return Json(categories);
        }//GetCategories

        // Добовление автора
        [Authorize(Roles = "Admin")]
        public IActionResult CategoryInsert(string CategoryName)
        {

            string ResponseText;
            bool IsSuccess;

            Category category = new Category();

            // Проверка на наличие в БД
            using (ApplicationContext db = new ApplicationContext())
                category = db.сategories.FirstOrDefault(a => a.CategoryName == CategoryName);

            if (category == null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.сategories.Add(new Category(CategoryName, true));
                    db.SaveChanges();
                }
                ResponseText = "Жанр успешно добавлен";
                IsSuccess = true;

            }
            else
            {
                ResponseText = "Жанр уже есть в списке";
                IsSuccess = false;
            }


            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//CategoryInsert

        // Изменить автора
        [Authorize(Roles = "Admin")]
        public IActionResult CategoryUpdate(Category category)
        {
            string ResponseText;
            bool IsSuccess;

            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (category != null)
                        db.сategories.Update(category);

                    db.SaveChanges();
                    ResponseText = "Данные успешно изменены";
                    IsSuccess = true;
                }//using
            }
            catch
            {
                ResponseText = "Ошибка на сервере.";
                IsSuccess = false;

            }//try-catch

            return Json(new { success = IsSuccess, responseText = ResponseText });
        }//CategoryUpdate

    }
}