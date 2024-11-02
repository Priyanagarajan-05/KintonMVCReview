using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MvcCourseManagement.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Content { get; set; }

        [Required]
        public int ProfessorId { get; set; } // FK to User

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsApproved { get; set; } = false;

        public int EnrollmentCount { get; set; } = 0;

        public int Version { get; set; } = 1; // Default to 1

        [JsonIgnore]
        public List<Module> Modules { get; set; } = new List<Module>();
    }
}
