using MvcCourseManagement.Models;
using System.ComponentModel.DataAnnotations;
namespace MvcCourseManagement.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; } // FK to Course

        [Required]
        public int StudentId { get; set; } // FK to User

        public int? Rating { get; set; } // Nullable rating
        
        public int CourseVersion { get; set; } // Version of the course when student enrolled

        public bool IsCompleted { get; set; } = false; // Track completion status

        // Add navigation property to the Course
        public Course Course { get; set; } // Navigation property to Course
    }
}