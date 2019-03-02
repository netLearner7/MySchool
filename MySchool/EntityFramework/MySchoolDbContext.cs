using Microsoft.EntityFrameworkCore;
using MySchool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.EntityFramework
{
    public class MySchoolDbContext: DbContext
    {
        public MySchoolDbContext(DbContextOptions<MySchoolDbContext> options) : base(options) {

        }

        public DbSet<Student> Students { get; set; }
        
        public DbSet<Course> Courses { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Course>().ToTable("Course").Property(a => a.CourseId).ValueGeneratedNever();
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasOne(a => a.Course).WithMany(a => a.Enrollments);
            modelBuilder.Entity<Enrollment>().HasOne(a => a.Student).WithMany(a => a.Enrollments);

        }
    }
}
