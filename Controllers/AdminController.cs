

using Microsoft.AspNetCore.Mvc;
using MvcCourseManagement.Data;
using MvcCourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcCourseManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            var students = users.Where(u => u.Role == "Student").ToList();
            var professors = users.Where(u => u.Role == "Professor").ToList();

            var viewModel = new AdminDashboardViewModel
            {
                Students = students,
                ApprovedProfessors = professors.Where(p => p.IsActive && !p.IsSuspend).ToList(),
                PendingUsers = users.Where(u => !u.IsActive && !u.IsSuspend).ToList(),
                SuspendedProfessors = professors.Where(p => p.IsSuspend).ToList(),
                CoursesPendingApproval = _context.Courses.Where(c => !c.IsApproved).ToList() // Add courses pending approval
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AcceptUser([FromBody] UserUpdateRequest request)
        {
            var user = _context.Users.Find(request.UserId);
            if (user == null)
                return NotFound("User not found.");

            user.IsActive = true;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult RejectUser([FromBody] UserUpdateRequest request)
        {
            var user = _context.Users.Find(request.UserId);
            if (user == null)
                return NotFound("User not found.");

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult SuspendProfessor([FromBody] SuspendProfessorRequest request)
        {
            var professor = _context.Users.Find(request.UserId);
            if (professor == null || professor.Role != "Professor")
                return NotFound("Professor not found.");

            professor.IsSuspend = true;
            professor.SuspensionStartDate = request.StartDate;
            professor.SuspensionEndDate = request.EndDate;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult RevokeProfessor([FromBody] UserUpdateRequest request)
        {
            var professor = _context.Users.Find(request.UserId);
            if (professor == null || professor.Role != "Professor" || !professor.IsSuspend)
                return NotFound("Suspended professor not found.");

            professor.IsSuspend = false;
            _context.SaveChanges();
            return Ok();
        }

        // Accept Course
        [HttpPost]
        public IActionResult AcceptCourse([FromBody] CourseUpdateRequest request)
        {
            var course = _context.Courses.Find(request.CourseId);
            if (course == null)
                return NotFound("Course not found.");

            course.IsApproved = true;
            _context.SaveChanges();
            return Ok();
        }

        // Reject Course
        [HttpPost]
        public IActionResult RejectCourse([FromBody] CourseUpdateRequest request)
        {
            var course = _context.Courses.Find(request.CourseId);
            if (course == null)
                return NotFound("Course not found.");

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return Ok();
        }
    }

    public class UserUpdateRequest
    {
        public int UserId { get; set; }
    }

    public class CourseUpdateRequest
    {
        public int CourseId { get; set; }
    }

    public class SuspendProfessorRequest : UserUpdateRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public List<User> Students { get; set; }
        public List<User> ApprovedProfessors { get; set; }
        public List<User> SuspendedProfessors { get; set; }
        public List<User> PendingUsers { get; set; }
        public List<Course> CoursesPendingApproval { get; set; } // Add for courses pending approval
    }
}

