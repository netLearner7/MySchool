using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Core.Models
{
    public class people
    {
        public int Id { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "不可以超过8个字符")]
        [Display(Name = "姓名")]
        public string RealName { get; set; }
    }
}
