using Microsoft.AspNetCore.Mvc;
using MvcCourseManagement.Data;
using MvcCourseManagement.Models;
using System.Linq;

namespace MvcCourseManagement.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            // Fetch the courses for the logged-in professor
            var professorId = (int)HttpContext.Session.GetInt32("UserId");
            var courses = _context.Courses
                                  .Where(c => c.ProfessorId == professorId)
                                  .ToList();

            ViewBag.MyCourses = courses; // Pass courses to the view

            return View();
        }

        // GET: /Professor/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View(new CourseViewModel());
        }

        // POST: /Professor/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new course
                var course = new Course
                {
                    Title = model.CourseTitle,
                    Description = model.CourseDescription,
                    Content = model.CourseContent,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Price = model.Price,
                    ProfessorId = (int)HttpContext.Session.GetInt32("UserId"), // Get the Professor ID from session
                    IsApproved = false // Course needs admin approval
                };

                _context.Courses.Add(course);
                _context.SaveChanges();

                // Add modules to the course if provided
                if (model.Modules != null)
                {
                    foreach (var module in model.Modules)
                    {
                        var courseModule = new Module
                        {
                            Title = module.Title,
                            Content = module.Content,
                            CourseId = course.CourseId,
                            Order = module.Order
                        };
                        _context.Modules.Add(courseModule);
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("Dashboard", "Professor");
            }
            return View(model);
        }
    }
}
