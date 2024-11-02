using System.ComponentModel.DataAnnotations;

namespace MvcCourseManagement.Models
{
    public class CourseViewModel
    {
        [Required]
        public string CourseTitle { get; set; }

        [Required]
        public string CourseDescription { get; set; }

        public string CourseContent { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid price")]
        public decimal Price { get; set; }

        public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>();
    }

    public class ModuleViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public int Order { get; set; } // Order of module in course
    }
}