using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool.Application.Dtos;
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
        public async Task<IActionResult> Index()
        {
            //查询出学生表的所有信息
            return View(await _context.Students.AsNoTracking().ToListAsync());
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
