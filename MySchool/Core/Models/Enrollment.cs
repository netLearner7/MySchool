using MySchool.Application.enumsType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    //学生表和课程表的关联表
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public CourseGrade? Grade { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
    }
}
