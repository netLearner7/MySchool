using MySchool.Application.enumsType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    //课程表
    public class Course
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public int Credits { get; set; }

       

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
