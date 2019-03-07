using MySchool.Application.enumsType;
using MySchool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.EntityFramework.Data
{
    public class DbInitializer
    {

            public static void Initialize(MySchoolDbContext context)
            {
                //    codefirst的时候如果没有库，则取消掉这句注释建库
                //    context.Database.EnsureCreated();

                // 检查是否有学生信息
                if (context.Students.Any())
                {
                    return; //返回，不执行。
                }


                #region 添加种子学生信息

                var students = new[]
                {
                new Student {RealName = "龙傲天", EnrollmentDate = DateTime.Parse("2005-09-01")},
                new Student {RealName = "王尼玛", EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student {RealName = "张全蛋", EnrollmentDate = DateTime.Parse("2003-09-01")},
                new Student {RealName = "叶良辰", EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student {RealName = "和珅", EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student {RealName = "纪晓岚", EnrollmentDate = DateTime.Parse("2001-09-01")},
                new Student {RealName = "李逍遥", EnrollmentDate = DateTime.Parse("2003-09-01")},
                new Student {RealName = "王小虎", EnrollmentDate = DateTime.Parse("2005-09-01")}
            };
                foreach (var s in students)
                    context.Students.Add(s);
                context.SaveChanges();

                #endregion


                #region 添加种子老师信息

                var Instructor = new[]
                {
                new Instructor
                {
                    RealName = "孔子",
                    HrieDate = DateTime.Parse("1995-03-11")
                },
                new Instructor
                {
                    RealName = "墨子",
                    HrieDate = DateTime.Parse("2003-03-11")
                },
                new Instructor
                {
                    RealName = "荀子",
                    HrieDate = DateTime.Parse("1990-03-11")
                },
                new Instructor
                {
                    RealName = "鬼谷子",
                    HrieDate = DateTime.Parse("1985-03-11")
                },
                new Instructor
                {
                    RealName = "孟子",
                    HrieDate = DateTime.Parse("2003-03-11")
                },
                new Instructor
                {
                    RealName = "朱熹",
                    HrieDate = DateTime.Parse("2003-03-11")
                }
            };

                foreach (var i in Instructor)
                    context.Instructor.Add(i);
                context.SaveChanges();

                #endregion


                #region 添加部门的种子的数据

                var Department = new[]
                {
                new Department
                {
                    name = "论语",
                    Budget = 350000,
                    StarDate = DateTime.Parse("2017-09-01"),
                    InstructorId = Instructor.Single(i => i.RealName == "孟子").Id
                },
                new Department
                {
                    name = "兵法",
                    Budget = 100000,
                    StarDate = DateTime.Parse("2017-09-01"),
                    InstructorId = Instructor.Single(i => i.RealName == "鬼谷子").Id
                },
                new Department
                {
                    name = "文言文",
                    Budget = 350000,
                    StarDate = DateTime.Parse("2017-09-01"),
                    InstructorId = Instructor.Single(i => i.RealName == "朱熹").Id
                },
                new Department
                {
                    name = "世界和平",
                    Budget = 100000,
                    StarDate = DateTime.Parse("2017-09-01"),
                    InstructorId = Instructor.Single(i => i.RealName == "墨子").Id
                }
            };

                foreach (var d in Department)
                    context.Department.Add(d);
                context.SaveChanges();

                #endregion


                var courses = new[]
                {
                new Course
                {
                    CourseId = 1050,
                    Title = "数学",
                    Credits = 3,
                    DepartmentId = Department.Single(s => s.name == "兵法").Id
                },
                new Course
                {
                    CourseId = 4022,
                    Title = "政治",
                    Credits = 3,
                    DepartmentId = Department.Single(s => s.name == "文言文").Id
                },
                new Course
                {
                    CourseId = 4041,
                    Title = "物理",
                    Credits = 3,
                    DepartmentId = Department.Single(s => s.name == "兵法").Id
                },
                new Course
                {
                    CourseId = 1045,
                    Title = "化学",
                    Credits = 4,
                    DepartmentId = Department.Single(s => s.name == "世界和平").Id
                },
                new Course
                {
                    CourseId = 3141,
                    Title = "生物",
                    Credits = 4,
                    DepartmentId = Department.Single(s => s.name == "论语").Id
                },
                new Course
                {
                    CourseId = 2021,
                    Title = "英语",
                    Credits = 3,
                    DepartmentId = Department.Single(s => s.name == "论语").Id
                },
                new Course
                {
                    CourseId = 2042,
                    Title = "历史",
                    Credits = 4,
                    DepartmentId = Department.Single(s => s.name == "文言文").Id
                }
            };


                foreach (var c in courses)
                    context.Course.Add(c);
                context.SaveChanges();


                #region 办公室分配的种子数据

                var officeAssignments = new[]
                {
                new OfficeAssignment
                {
                    InstructorId = Instructor.Single(i => i.RealName == "孟子").Id,
                    Location = "逸夫楼 17"
                },
                new OfficeAssignment
                {
                    InstructorId = Instructor.Single(i => i.RealName == "朱熹").Id,
                    Location = "青霞路 27"
                },
                new OfficeAssignment
                {
                    InstructorId = Instructor.Single(i => i.RealName == "墨子").Id,
                    Location = "天府楼 304"
                }
            };

                foreach (var o in officeAssignments)
                    context.OfficeAssignment.Add(o);
                context.SaveChanges();

                #endregion

                #region 课程老师的种子数据

                var courseInstructors = new[]
                {
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "数学").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "鬼谷子").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "数学").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "墨子").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "政治").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "朱熹").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "化学").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "墨子").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "生物").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "孟子").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "英语").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "孟子").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "物理").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "鬼谷子").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "历史").CourseId,
                    InstructorId = Instructor.Single(i => i.RealName == "朱熹").Id
                }

            };

                foreach (var ci in courseInstructors)
                    context.CourseAssignment.Add(ci);
                context.SaveChanges();

                #endregion


                var enrollments = new[]
                {
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "龙傲天").Id,
                    CourseId = courses.Single(c => c.Title == "数学").CourseId,
                    Grade = CourseGrade.A
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "龙傲天").Id,
                    CourseId = courses.Single(c => c.Title == "政治").CourseId,
                    Grade = CourseGrade.C
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "龙傲天").Id,
                    CourseId = courses.Single(c => c.Title == "物理").CourseId,
                    Grade = CourseGrade.D
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "王尼玛").Id,
                    CourseId = courses.Single(c => c.Title == "物理").CourseId,
                    Grade = CourseGrade.F
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "王尼玛").Id,
                    CourseId = courses.Single(c => c.Title == "化学").CourseId
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "王尼玛").Id,
                    CourseId = courses.Single(c => c.Title == "生物").CourseId
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "叶良辰").Id,
                    CourseId = courses.Single(c => c.Title == "英语").CourseId,
                    Grade = CourseGrade.A
                }, new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "叶良辰").Id,
                    CourseId = courses.Single(c => c.Title == "历史").CourseId,
                    Grade = CourseGrade.D
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "张全蛋").Id,
                    CourseId = courses.Single(c => c.Title == "英语").CourseId,
                    Grade = CourseGrade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "张全蛋").Id,
                    CourseId = courses.Single(c => c.Title == "数学").CourseId,
                    Grade = CourseGrade.A
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "纪晓岚").Id,
                    CourseId = courses.Single(c => c.Title == "英语").CourseId
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "王小虎").Id,
                    CourseId = courses.Single(c => c.Title == "生物").CourseId
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "和珅").Id,
                    CourseId = courses.Single(c => c.Title == "物理").CourseId,
                    Grade = CourseGrade.A
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.RealName == "和珅").Id,
                    CourseId = courses.Single(c => c.Title == "英语").CourseId
                }
            };
                foreach (var e in enrollments)
                    context.Enrollment.Add(e);
                context.SaveChanges();
            }
        }
}
