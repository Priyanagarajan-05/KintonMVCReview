using Microsoft.AspNetCore.Mvc;
using MvcCourseManagement.Data;
using MvcCourseManagement.Models;

namespace MvcCourseManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Ensure IsActive is set to false by default and IsSuspend is false
                user.IsActive = false;
                user.IsSuspend = false;

                _context.Users.Add(user);
                _context.SaveChanges();  // Save user to the database

                // Redirect to Login page after successful registration
                return RedirectToAction("Login", "User");
            }

            return View(user);
        }

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
       
        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and Password are required.";
                return View();
            }

            // Retrieve the user from the database based on email and password
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid login attempt.";
                return View();
            }

            // Check if the user is suspended or inactive
            if (user.IsSuspend)
            {
                ViewBag.Error = "Your account is suspended.";
                return View();
            }
            if (!user.IsActive)
            {
                ViewBag.Error = "Your account is not active.";
                return View();
            }

            // Set user details in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetInt32("UserId", user.UserId); // Store UserId in session

            // Redirect based on the role
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                case "Professor":
                    return RedirectToAction("Dashboard", "Professor");
                case "Student":
                    return RedirectToAction("Dashboard", "Student");
                default:
                    ViewBag.Error = "Invalid role.";
                    return View();
            }
        }


        // POST: /User/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        // GET: /User/EditProfile
        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /User/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(updatedUser.UserId);
                if (user != null)
                {
                    user.Name = updatedUser.Name;
                    user.Email = updatedUser.Email;
                    user.Password = updatedUser.Password;
                    user.Role = updatedUser.Role;
                    user.IsSuspend = updatedUser.IsSuspend;

                    _context.SaveChanges();
                    return RedirectToAction("Profile", new { id = user.UserId });
                }
            }
            return View(updatedUser);
        }
    }
}
