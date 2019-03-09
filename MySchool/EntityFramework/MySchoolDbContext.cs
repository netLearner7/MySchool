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

        public DbSet<Course> Course { get; set; }

        public DbSet<CourseAssignment> CourseAssignment { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Enrollment> Enrollment { get; set; }

        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<OfficeAssignment> OfficeAssignment { get; set; }

        public DbSet<people> people { get; set; }

        public DbSet<Student> Students { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Student>().ToTable("Student");

            modelBuilder.Entity<Course>().ToTable("Course").Property(a => a.CourseId).ValueGeneratedNever();

            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment").HasKey(b=>new { b.CourseId,b.InstructorId});

            modelBuilder.Entity<Department>().ToTable("Department");

            modelBuilder.Entity<Instructor>().ToTable("Instructor");

            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment").HasKey(c=>c.InstructorId);

            modelBuilder.Entity<people>().ToTable("people");
                

            modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasOne(d => d.Course).WithMany(d => d.Enrollments);
            modelBuilder.Entity<Enrollment>().HasOne(e => e.Student).WithMany(e =>e.Enrollments);

        }
    }
}
