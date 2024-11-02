using Microsoft.EntityFrameworkCore;
using MvcCourseManagement.Models;
using System;

namespace MvcCourseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("Student");

            modelBuilder.Entity<Module>()
                .HasOne(m => m.Course)
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .Property(c => c.IsApproved)
                .HasDefaultValue(false);

            modelBuilder.Entity<Course>()
                .Property(c => c.EnrollmentCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<Course>()
                .Property(c => c.Version)
                .HasDefaultValue(1); // Default version

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.CourseVersion)
                .HasDefaultValue(1); // Default version
        }
    }
}
