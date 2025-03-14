﻿using System.ComponentModel.DataAnnotations;

namespace MvcCourseManagement.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public int CourseId { get; set; } // FK to Course

        public int CourseVersion { get; set; } // Version of the course

        public Course Course { get; set; } // Navigation property to Course

        public int Order { get; set; } // To determine module order
    }
}
