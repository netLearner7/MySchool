using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    //课程分配表
    public class CourseAssignment
    {
        public int InstructorId { get; set; }

        public int CourseId { set; get; }

        public Instructor instructors { get; set; }

        public Course courses { get; set; }
    }
}
