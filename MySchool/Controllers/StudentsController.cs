using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool.Application.Dtos;
using MySchool.Common;
using MySchool.Core.Models;
using MySchool.EntityFramework;

namespace MySchool.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MySchoolDbContext _context;

        public StudentsController(MySchoolDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult>  Index
            (string sortOrder,string SearchStudents,int? page,string CurrentStudent,string logo)
        {
            //姓名排序参数
            ViewData["Name_Sort_Parm"] = string.IsNullOrEmpty(sortOrder) ? "name desc" : "";           
            //时间排序参数
            ViewData["Date_Sort_Parm"] = sortOrder == "date" ? "date desc" : "date";
            //搜索参数
            ViewData["SearchStudents"] = SearchStudents;

            


            //判断搜索学生是否为空如果不为空则表示进行搜索，所以将页号定位到1
            if (SearchStudents != null)
            {
                page = 1;
            }
            else
            {
                //如果为空，则会出现没有搜索和搜索中翻页两种情况
                //没有搜索的话两个参数都为空，不会影响到结果
                //搜索中翻页的话则视图只能传递CurrentStudent参数所以赋值给SearchStudents，
                //使其筛选出合适的数据供CreatePagng进行偏移量操作
                SearchStudents = CurrentStudent;
            }

            //读取出整张表
            var students = from Student in _context.Students select Student;

            #region 搜索和排序功能

            //先做筛选，否则会影响到排序和翻页（偏移量操作）
            if (!string.IsNullOrWhiteSpace(SearchStudents)) {
                students = students.Where(a => a.RealName.Contains(SearchStudents));
                //每次搜索时将搜索学生设置为当前学生
                ViewData["CurrentStudent"] = SearchStudents;
            }


            //在做排序，否则影响翻页。
            //排序分为排序，排序翻页，筛选排序翻页
            //第一种sortOrder会将值传递回来则直接进行排序
            //第2 3中不会传递sortOrder因为一次sortOrder到后台时都是会变化的取决于姓名排序参数和时间排序参数
            //所以当第一次排序时，将排序方式用logo记录下来并传递到view，前台翻页时传递logo，根据logo完成原来的排序，此时sortOrder为null
            if (sortOrder != null)
            {
                students= Sort(sortOrder, students);
            }
            else
            {
                students = Sort(logo, students);
            }
            
            #endregion

            //设定每页的大小
            int PageSize = 3;

            //读出筛选后的所有数据
            var entities= students.AsNoTracking();

            //这个类以继承list方式组成1个list+4个属性的数据结构，存放相关数据
            var dtos =await  PaginatedList<Student>.CreatePagng(entities, page ?? 1, PageSize);

            //var dtos = await students.ToListAsync();
            //查询出学生表的所有信息
            return View(dtos);
        }


        public IQueryable<Student> Sort(string logo,IQueryable<Student> students) {
            switch (logo)
            {
                case "name desc":
                    ViewData["logo"] = "name desc";
                    return students.OrderByDescending(a => a.RealName);
                  
                case "date desc":
                    ViewData["logo"] = "date desc";
                    return students.OrderByDescending(a => a.EnrollmentDate);
                    
                case "date":
                    ViewData["logo"] = "date desc";
                    return students.OrderBy(a => a.EnrollmentDate);
                    
                default:
                    ViewData["logo"] = "name";
                    return students.OrderBy(a => a.RealName);              
            };

        }



        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //id不存在则返回404
            if (id == null)
            {
                return NotFound();
            }

            //查询出id相关的信息
            var student = await _context.Students.Include(a => a.Enrollments).ThenInclude(e => e.Course).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            //如果查询不到返回404
            if (student == null)
            {
                return NotFound();
            }

            //携带信息返回视图
            return View(student);
        }

        // GET: Students/Create
        /// <summary>
        /// 打开添加页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //防止csrf
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDto Dto)
        {

            try
            {
                //数据验证，判断数据是否符合特性要求，比如tdo模型中特性要求某个属性不能为空等
                if (ModelState.IsValid)
                {
                    //从tdo模型绑定到model
                    var entity = new Student()
                    {
                        RealName = Dto.RealName,
                        EnrollmentDate = Dto.EnrollmentDate
                    };
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (DbUpdateException ex)
            {
                //给数据模型添加一个错误信息可以在页面中显示
                ModelState.AddModelError("", "无法保存，请检查输入的数据！");
            }


            return View(Dto);
        }

        // GET: Students/Edit/5
        [ActionName("Edit")]

        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                //404
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id,RealName,EnrollmentDate")] Student student)
        {
            if (student.Id==0)
            {
                return NotFound();
            }
            var entity =await _context.Students.SingleOrDefaultAsync(a=>a.Id==student.Id);

            //尝试将某个对象更新到student的模型中
            if (await TryUpdateModelAsync<Student>(entity, "", b => b.EnrollmentDate, b => b.RealName)) {

                try
                {
                    await _context.SaveChangesAsync();
                    return  RedirectToAction(nameof(Index));
                   
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "无法保存，请检查输入的数据！");
                }

            }
            
          
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id,bool? SaveChange=false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            if (SaveChange == true) {
                ViewBag.error = "保存失败！";
            }


            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (student == null) {
                //如果查询不到则返回delete方法并且利用savechang在页面上显示错误信息
                return RedirectToAction(nameof(Delete), new { id = id, SaveChange = true });
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                return RedirectToAction(nameof(Delete), new { id=id, SaveChange =true});
            }
           
            
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
