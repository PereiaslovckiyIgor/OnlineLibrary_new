using System.Linq;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            Statistics statistics = new Statistics();

            using (ApplicationContext db = new ApplicationContext())
                statistics = db.statistics.FromSql($"EXECUTE [admin].[getStatistics]").FirstOrDefault();


            return View(statistics);
        }
    }
}