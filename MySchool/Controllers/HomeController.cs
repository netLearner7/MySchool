using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySchool.EntityFramework;
using MySchool.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Data.Common;

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


        //ef使用原生sql贼麻烦
        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "学生统计信息";


            var groups = new List<EnrollmentDateGroup>();


            var conn = _dbContext.Database.GetDbConnection();


            try
            {
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                    string sqlQuery = @"select  EnrollmentDate,  COUNT(*) as StudentCount     from people where Discriminator='Student' group by EnrollmentDate";

                    command.CommandText = sqlQuery;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroup()
                            {
                                EnrollmentDate = reader.GetDateTime(0),
                                StudentCount = reader.GetInt32(1)
                            };
                            groups.Add(row);

                        }
                    }
                    reader.Dispose();
                }

            }
            finally
            {
                conn.Close();
            }
            return View(groups);
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
