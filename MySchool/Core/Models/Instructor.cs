using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    /// <summary>
    /// 教师表
    /// </summary>
    public class Instructor:people
    {
        //工龄
        [Display(Name ="入职时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HrieDate { get; set; }

        public ICollection<CourseAssignment> courseAssignments { get; set; }

        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
