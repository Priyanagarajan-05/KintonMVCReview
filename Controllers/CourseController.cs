/* ===== fine==
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCourseManagement.Data;
using MvcCourseManagement.Models;

namespace MvcCourseManagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SubmitCourse(Course course)
        {
            course.IsApproved = false; // Set to false initially
            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Index"); // Redirect to course listing page or dashboard
        }


        // POST: /Course/BuyCourse
        [HttpPost]
        public IActionResult BuyCourse(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = (int)userId,
                CourseVersion = 1 // Assuming versioning, adjust as necessary
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return RedirectToAction("CourseContent", new { courseId, moduleIndex = 0 }); // Redirect to CourseContent
        }
        public IActionResult CourseContent(int courseId, int moduleIndex)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var course = _context.Courses.Include(c => c.Modules).FirstOrDefault(c => c.CourseId == courseId);
            if (course == null || moduleIndex >= course.Modules.Count)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollments.FirstOrDefault(e => e.CourseId == courseId && e.StudentId == userId);
            if (enrollment == null)
            {
                return Unauthorized();
            }

            // Check if the course is completed
            bool isLastModule = moduleIndex == course.Modules.Count - 1;
            if (isLastModule)
            {
                enrollment.IsCompleted = true;
                _context.SaveChanges();
            }

            var model = new
            {
                Course = course,
                Module = course.Modules[moduleIndex],
                NextModuleIndex = moduleIndex + 1,
                IsCompleted = enrollment.IsCompleted
            };

            return View(model);
        }

        // POST: /Course/SubmitRating
        [HttpPost]
        public IActionResult SubmitRating(int courseId, int rating)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var enrollment = _context.Enrollments.FirstOrDefault(e => e.CourseId == courseId && e.StudentId == userId && e.IsCompleted);
            if (enrollment == null)
            {
                return NotFound();
            }

            enrollment.Rating = rating;
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Student");
        }

    }
}
