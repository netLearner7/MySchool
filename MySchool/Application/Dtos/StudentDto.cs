using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Application.Dtos
{
    public class StudentDto
    {
        
        public string RealName { get; set; }


        public DateTime EnrollmentDate { get; set; }
    }
}
