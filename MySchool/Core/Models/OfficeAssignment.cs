using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    /// <summary>
    /// 办公室表
    /// </summary>
    public class OfficeAssignment
    {
        [Key]
        public int InstructorId { get; set; }

        public Instructor instructor { get; set; }

        [StringLength(50)]
        [Display(Name ="Office Location")]
        public string Location { get; set; }
    }
}
