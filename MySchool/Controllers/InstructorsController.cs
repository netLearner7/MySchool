using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool.Core.Models;
using MySchool.EntityFramework;
using MySchool.ViewModels;

namespace MySchool.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly MySchoolDbContext _context;

        public InstructorsController(MySchoolDbContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(int? id,int? courseId)
        {

            InstructorIndexData viewmodel = new InstructorIndexData();
            //根据这个老师筛选出所有有关信息
            viewmodel.instructors = _context.Instructor.Include(a => a.OfficeAssignment).
                Include(a => a.courseAssignments).ThenInclude(a => a.courses).
                ThenInclude(a => a.Enrollments).ThenInclude(a => a.Student).
                Include(a => a.courseAssignments).ThenInclude(a => a.courses).
                ThenInclude(a => a.Department).OrderBy(a => a.RealName).AsNoTracking().ToList();



            if (id != null) {
                ViewData["InstructorId"] = id.Value;
                var instructor = viewmodel.instructors.Single(a => a.Id == id.Value);
                viewmodel.courses = instructor.courseAssignments.Select(a => a.courses).ToList();
            }

            if (courseId != null) {
                ViewData["courseId"] = courseId.Value;
                viewmodel.enrollments= viewmodel.courses.Single(a => a.CourseId == courseId.Value).Enrollments.ToList();
            }

            return View(viewmodel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            var Instructor = new Instructor();
            var courseAssignments = new List<CourseAssignment>();
            Instructor.courseAssignments = courseAssignments;

            PopulateAssignedCourseData(Instructor);
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RealName,HrieDate,OfficeAssignment")] Instructor instructor
            ,string[] select)
        {

            if (select!=null)
            {
                instructor.courseAssignments = new List<CourseAssignment>();
                foreach (var item in select)
                {
                    var CourseAdd = new CourseAssignment() {
                        CourseId = Convert.ToInt32(item),
                        InstructorId = instructor.Id

                    };
                    instructor.courseAssignments.Add(CourseAdd);

                }

            }

            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor.Include(a=>a.OfficeAssignment).Include(a=>a.courseAssignments)
                .ThenInclude(a=>a.courses).SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ActionName(nameof(Edit))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id,string[] select)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructor.Include(a => a.OfficeAssignment)
                 .Include(a => a.courseAssignments)
                 .SingleOrDefaultAsync(s => s.Id == id);

            if (await TryUpdateModelAsync(instructorToUpdate, "",a=>a.RealName,a=>a.HrieDate,a=>a.OfficeAssignment)) { 

                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location)) {

                    instructorToUpdate.OfficeAssignment = null;
                }

                UpdateInstructorsCourse(select, instructorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    ModelState.AddModelError("", "保存异常！");
                }

                return RedirectToAction(nameof(Index));
            }
            UpdateInstructorsCourse(select, instructorToUpdate);
            PopulateAssignedCourseData(instructorToUpdate);

            return View(instructorToUpdate);


        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructor.Include(a=>a.courseAssignments).SingleOrDefaultAsync(m => m.Id == id);

            var departments = await _context.Department.Where(a => a.InstructorId == id).ToListAsync();
            departments.ForEach(a=>a.InstructorId=null);

            _context.Instructor.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #region 某个老师的课程表
        private void PopulateAssignedCourseData(Instructor instructor) {

            var CourseList = _context.Course;

            var InstructorCourse = new HashSet<int>(instructor.courseAssignments.Select(a=>a.CourseId));

            var viewmodel = new List<AssignedCourseData>();

            foreach (var item in CourseList)
            {
                viewmodel.Add(
                    new AssignedCourseData() {
                        CourseId = item.CourseId,
                        Title = item.Title,
                        Assigned = InstructorCourse.Contains(item.CourseId)
                    }
                    );
            }

            ViewData["Courses"] = viewmodel;



        }

        #endregion

        #region
        /// <summary>
        /// x修改老师的课程信息
        /// </summary>
        /// <param name="selectCourse">选中的课程</param>
        /// <param name="instructor">修改的老师</param>
        private void UpdateInstructorsCourse(string[] selectCourse, Instructor instructor) {
            if (selectCourse == null)
            {
                instructor.courseAssignments = new List<CourseAssignment>();
                return;
            }
            var selectcourseHS = new HashSet<string>(selectCourse);
            var instructorCourse = new HashSet<int>(instructor.courseAssignments.Select(a=>a.CourseId));

            foreach (var Course in _context.Course)
            {
                if (selectcourseHS.Contains(Course.CourseId.ToString()))
                {
                    if (!instructorCourse.Contains(Course.CourseId))
                    {
                        instructor.courseAssignments.Add(new CourseAssignment()
                        {
                            InstructorId = instructor.Id,
                            CourseId = Course.CourseId
                        });
                    }
                }
                else {
                    if (instructorCourse.Contains(Course.CourseId))
                    {
                        var CourseToRemove = instructor.courseAssignments.SingleOrDefault(a => a.CourseId == Course.CourseId);
                        _context.Remove(CourseToRemove);
                    }

                }

            }

        }

        #endregion

        private bool InstructorExists(int id)
        {
            return _context.Instructor.Any(e => e.Id == id);
        }
    }
}
