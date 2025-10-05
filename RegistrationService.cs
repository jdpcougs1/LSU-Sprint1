using System;
using System.Collections.Generic;
using System.Linq;

namespace LSU_App
{
    /// Modified 04OCT25 Karima
    /// <summary>
    /// Sprint 2, seat availability verification
    /// </summary>
    public sealed class RegistrationService
    {
        private readonly CourseCatalogService _catalog;
        private readonly AuthService _auth;
        private readonly List<Enrollment> _enrollments = new();

        /// Student record UserName, for completed courses
        private readonly HashSet<(string user, string course)> _completed = new();

        public RegistrationService(CourseCatalogService catalog, AuthService auth)
        {
            _catalog = catalog;
            _auth = auth;
        }

        /// Completed course counter
        public void AddCompleted(string studentUsername, string courseCode)
            => _completed.Add((studentUsername, courseCode));

        public IEnumerable<Enrollment> GetSchedule(string studentUsername)
            => _enrollments.Where(e => e.StudentUsername.Equals(studentUsername, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Course registration
        /// </summary>
        public (bool ok, string message) Register(string studentUsername, string courseCode)
        {
            var user = _auth.GetUser(studentUsername);
            if (user is null || user.Role != Role.Student)
                return (false, "Only student accounts can register.");

            var course = _catalog.Get(courseCode);
            if (course is null) return (false, $"Course {courseCode} not found.");

            // Is seat available
            if (!course.HasSeatAvailable())
                return (false, "Course is full.");

            // Student already enrolled
            if (_enrollments.Any(e => e.StudentUsername.Equals(studentUsername, StringComparison.OrdinalIgnoreCase) &&
                                      e.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase)))
                return (false, "You are already enrolled in this course.");

            // Prerequisite verification
            foreach (var pre in course.Prerequisites)
            {
                if (!_completed.Contains((studentUsername, pre)))
                    return (false, $"Missing prerequisite: {pre}.");
            }

            // Course registration complete
            course.Enrolled += 1;
            _enrollments.Add(new Enrollment { StudentUsername = studentUsername, CourseCode = courseCode });
            return (true, $"Registered in {course.Code} - {course.Title}.");
        }
    }
}