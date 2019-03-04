using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Application.Dtos
{
    public class StudentDto
    {
        [DisplayName("学生姓名")]
        public string RealName { get; set; }

        [DisplayName("入学时间")]
        public DateTime EnrollmentDate { get; set; }
    }
}
