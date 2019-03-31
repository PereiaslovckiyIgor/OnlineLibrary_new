using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentsController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Comments()
        {
            return View();
        }//Comments

        // Получить списко всех Отзывов
        [Authorize(Roles = "Admin")]
        public IActionResult GetComments()
        {
            List<Comment> comments = new List<Comment>();

            using (ApplicationContext db = new ApplicationContext())
                comments = db.comments.FromSql("EXECUTE [admin].[GetAllComments]").ToList();

            return Json(comments);
        }//GetComments

        // Получить текс отзыва
        [Authorize(Roles = "Admin")]
        public IActionResult GetCommentText(int IdComments) {

            string commText;
            using (ApplicationContext db = new ApplicationContext())
                commText = db.allComments.FirstOrDefault(comm => comm.IdComments == IdComments).CommentText;


                return Json(new { commentText = commText });
        }//GetCommentText



    }//CommentsController
}//namespace