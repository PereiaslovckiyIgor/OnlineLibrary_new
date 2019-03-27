using System.Linq;
using LibOnline.Areas.Admin.Models;
using LibOnline.Areas.Admin.Models.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            Statistics statistics = new Statistics();

            using (ApplicationContext db = new ApplicationContext())
                statistics = db.statistics.FromSql($"EXECUTE [admin].[getStatistics]").FirstOrDefault();


            return View(statistics);
        }
    }
}