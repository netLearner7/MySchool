using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    //学生表
    public class Student:people
    {
        [DisplayName("入学时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }


        [DisplayName("入学信息")]
        public ICollection<Enrollment> Enrollments { get; set; }

        [MaxLength(200)]
        public string secret { get; set; }
    }
}
