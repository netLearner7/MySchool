using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    /// <summary>
    /// 教师表
    /// </summary>
    public class Instructor
    {
        public int Id { get; set; }

        public string RealName { get; set; }

        //工龄
        public DateTime HrieDate { get; set; }

        public ICollection<CourseAssignment> courseAssignments { get; set; }

        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
