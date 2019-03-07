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
        [Required]
        [StringLength(8, ErrorMessage = "不可以超过8个字符")]
        [DisplayName("学生名称")]
        public string RealName { get; set; }

        [DisplayName("入学时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
    }
}
