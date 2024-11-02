using System.ComponentModel.DataAnnotations;
using System;

namespace MvcCourseManagement.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // Auto-incremented in SQL

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Principal key for relationship

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Professor, Student

        public bool IsActive { get; set; } = false;

        public bool IsSuspend { get; set; } = false; // Default to false

        public DateTime? SuspensionStartDate { get; set; } // Nullable, only set if user is suspended
        public DateTime? SuspensionEndDate { get; set; } // Nullable, only set if user is suspended
    }
}
