/* == fine ==
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCourseManagement.Data;
using MvcCourseManagement.Models;
using System.Linq;

namespace MvcCourseManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var approvedCourses = _context.Courses.Where(c => c.IsApproved).ToList();
            ViewBag.ApprovedCourses = approvedCourses;

            var enrolledCourses = _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == userId)
                .ToList();

            ViewBag.EnrolledCourses = enrolledCourses.Select(e => new
            {
                e.Course.Title,
                e.IsCompleted,
                e.Rating
            }).ToList();

            return View();
        }


        public IActionResult ViewModules(int courseId, int currentIndex = 0)
        {
            var course = _context.Courses
                                 .Include(c => c.Modules)
                                 .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound(); // Handle case where course is not found
            }

            // Store current index in ViewData for module display
            ViewData["CurrentModuleIndex"] = currentIndex;

            // Pass course and modules to the view
            return View(course);
        }


    }
}