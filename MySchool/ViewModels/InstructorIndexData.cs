using MySchool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.ViewModels
{
    public class InstructorIndexData
    {
        public List<Instructor> instructors { get; set; }

        public List<Course> courses { get; set; }

        public List<Enrollment> enrollments { get; set; }

    }
}
