using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    //学生表
    public class Student
    {
        public int Id { get; set; }

        [DisplayName("学生名称")]
        public string RealName { get; set; }

        [DisplayName("入学时间")]
        public DateTime EnrollmentDate { get; set; }

        [DisplayName("入学信息")]
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
