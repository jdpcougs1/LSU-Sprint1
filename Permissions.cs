using System;

namespace LSU_App
{
    /// Modified 04OCT25 Karima
    /// <summary>
    /// Sprint 2, user authentication verification
    /// </summary>
    public static class Permissions
    {
        public static bool CanManageCourses(Role role) => role == Role.Admin;
        public static bool CanReviewAdmissions(Role role) => role == Role.Admin || role == Role.Faculty;
        public static bool CanEnterGrades(Role role) => role == Role.Faculty;
        public static bool CanViewOwnGrades(Role role) => role == Role.Student;
    }
}