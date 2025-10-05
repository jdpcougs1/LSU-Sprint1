using System;

namespace LSU_App
{
    /// <summary>
    /// Represents a student's enrollment record for a given course (Sprint 2).
    /// </summary>
    public sealed class Enrollment
    {
        public string StudentUsername { get; set; } = ""; // ties to AuthService user name
        public string CourseCode { get; set; } = "";
        public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;

        public override string ToString() => $"{StudentUsername} -> {CourseCode} @ {EnrolledAtUtc:u}";
    }
}