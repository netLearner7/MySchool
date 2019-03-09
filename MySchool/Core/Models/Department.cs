using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name ="部门名称")]
        [StringLength(50, MinimumLength = 2)]
        public string name { set; get; }


        //预算
        [Column(TypeName = "Money")]
        [DataType(DataType.Currency)]
        [Display(Name = "预算")]
        public decimal Budget { get; set; }

        //开课时间
        [Display(Name = "开设时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StarDate { get; set; }

        //老师id
        public int? InstructorId { get; set; }

        //办公室主任
        [Display(Name = "办公室主任")]
        public Instructor Administrator { set; get; }

        //课程表
        public ICollection<Course> Courses{get;set;}

        //追踪版本号
        [Timestamp]
        public byte[] RowVersion { get; set; }
}
}
