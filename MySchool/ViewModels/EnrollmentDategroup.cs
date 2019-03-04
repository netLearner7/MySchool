using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.ViewModels
{
    public class EnrollmentDateGroup
    {
        [DisplayName("学生总数")]
        public int StudentCount { get; set; }

        [DisplayName("学生注册日期")]
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
    }
}
