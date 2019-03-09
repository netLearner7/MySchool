using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool.Core.Models;
using MySchool.EntityFramework;

namespace MySchool.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly MySchoolDbContext _context;

        public DepartmentsController(MySchoolDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var mySchoolDbContext = _context.Department.Include(d => d.Administrator);
            return View(await mySchoolDbContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string sql = "select * from Department where Id={0}";

            var department = await _context.Department.FromSql(sql,id)
                .Include(d => d.Administrator)
                .SingleOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "RealName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,Budget,StarDate,InstructorId,RowVersion")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id", department.InstructorId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department.Include(a=>a.Administrator).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            ViewBag.InstructorId = new SelectList(_context.Instructor, "Id", "Id", department.InstructorId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,byte[] RowVersion)
        {
            if (id==null)
            {
                return NotFound();
            }

            var department = await _context.Department.Include(a => a.Administrator).SingleOrDefaultAsync(a => a.Id == id);

            if (department == null) {
                var Delete = new Department();
                await TryUpdateModelAsync(Delete);
                ModelState.AddModelError(string.Empty, "信息可能已被其他人删除，请重试！");
                ViewData["instructorId"] = new SelectList(_context.Instructor,"Id", "RealName",Delete.InstructorId);
                return View(Delete);
;            }
            
            _context.Entry(department).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync(department,"",b=>b.name, b=>b.StarDate,b=>b.InstructorId,b=>b.Budget))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    var excepyionEntity = ex.Entries.Single();
                    //返回ef中追踪的对象，ef在内存中
                    var clientValue = (Department)excepyionEntity.Entity;
                    //返回数据库中的内容/对象
                    var databaseEntity = excepyionEntity.GetDatabaseValues();

                    if (databaseEntity==null)
                    {
                        ModelState.AddModelError(string.Empty, "信息可能已被其他人删除，请重试！");
                    }
                    else
                    {
                        var databaseValue = (Department)databaseEntity.ToObject();
                        if (databaseValue.name != clientValue.name) {

                            ModelState.AddModelError("name", $"数值错误：{databaseValue.name}");
                        }
                        if (databaseValue.StarDate != clientValue.StarDate)
                        {

                            ModelState.AddModelError("StarDate", $"数值错误：{databaseValue.StarDate}");
                        }
                        if (databaseValue.Budget != clientValue.Budget)
                        {

                            ModelState.AddModelError("Budget", $"数值错误：{databaseValue.Budget}");
                        }
                        //可能存在有人把老师给删了级联操作改动部门（可能老师时部门主任或者更换主任）所以判断老师是否存在
                        if (databaseValue.InstructorId != clientValue.InstructorId)
                        {
                            //查询老师信息
                            var InstuctorEntity = await _context.Instructor.SingleOrDefaultAsync(a => a.Id == databaseValue.Id);

                            //用id进行数据库操作，用姓名显示
                            ModelState.AddModelError("InstructorId", $"数值错误：{InstuctorEntity?.RealName}");
                        }

                        ModelState.AddModelError("", $"您的操作已经被其他用户提前修改，请刷新页面使用修改后的值进行操作！");
                        department.RowVersion = (byte[])databaseValue.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                   
                }
            }
            
            ViewBag.InstructorId= new SelectList(_context.Instructor, "Id", "RealName", department.InstructorId);
            

            return View(department);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <param name="concurrencyErroy">是否触发并发错误</param>
        /// <returns></returns>
        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id,bool? concurrencyErroy)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .Include(d => d.Administrator).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                if (concurrencyErroy.GetValueOrDefault())
                {
                    RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            if (concurrencyErroy.GetValueOrDefault())
            {
                ViewBag.concurrencyErroy = "信息已经被其他人修改";
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Department department)
        {

            try
            {
                if (await _context.Department.AnyAsync(a=>a.Id== department.Id))
                {
                    _context.Department.Remove(department);
                    await _context.SaveChangesAsync();
                   
                }
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException ex)
            {

                return RedirectToAction(nameof(Delete),new { id=department.Id,concurrencyErroy=true });
            }



           
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
