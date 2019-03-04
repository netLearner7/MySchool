using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySchool.EntityFramework;
using MySchool.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MySchool.Controllers
{
    public class HomeController : Controller
    {
     
        private MySchoolDbContext _dbContext;

        public HomeController(MySchoolDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "学生统计信息";

            var dots = from entity in _dbContext.Students
                       group entity by entity.EnrollmentDate into dateGroup
                       select new EnrollmentDateGroup()
                       {
                           StudentCount = dateGroup.Count(),
                           EnrollmentDate = dateGroup.Key
                       };
            var dtos = await dots.ToListAsync();
            
            return View(dtos);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
