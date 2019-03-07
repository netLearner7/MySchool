using MySchool.Application.enumsType;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    //课程表
    public class Course
    {
        [Display(Name ="课程号")]
        public int CourseId { get; set; }

        [Display(Name ="课程名称")]
        [StringLength(50,MinimumLength =3)]
        public string Title { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        [Range(0, 5)]
        [Display(Name = "课程学分")]
        public int Credits { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        [Display(Name = "课程成绩")]
        public CourseGrade? Grade { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int DepartmentId { get; set; }

        [Display(Name = "部门信息")]
        public Department Department { get; set; }

        public ICollection<CourseAssignment> courseAssignments { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
