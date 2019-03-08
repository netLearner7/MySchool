using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.ViewModels
{
    /// <summary>
    /// 分配课程数据
    /// </summary>
    public class AssignedCourseData
    {
        //课程Id
        public int CourseId { get; set; }

        //课程名称
        public string Title { get; set; }

        //是否分配
        public bool Assigned { get; set; }
    }
}
