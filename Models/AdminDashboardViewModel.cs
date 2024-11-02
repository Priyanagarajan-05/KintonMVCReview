namespace MvcCourseManagement.Models
{
    public class AdminDashboardViewModel
    {
        public List<User> Students { get; set; }
        public List<User> ApprovedProfessors { get; set; }
        public List<User> PendingUsers { get; set; }
        public List<User> SuspendedProfessors { get; set; } // Add this line
        public List<Course> PendingCourses { get; set; } // New field for pending courses
    }
}
