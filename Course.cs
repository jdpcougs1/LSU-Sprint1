using System;
using System.Collections.Generic;

namespace LSU_App
{
    /// Modified 03OCT25 Karima
    /// <summary>
    /// Sprint 2, Course Catalog
    /// </summary>
    public sealed class Course
    {
        public string Code { get; set; } = "";       // example: CISS-202
        public string Title { get; set; } = "";
        public string Department { get; set; } = "";
        public int Capacity { get; set; } = 30;
        public int Enrolled { get; set; } = 0;
        public int Credits { get; set; } = 3;

        /// Prerequisite course code list
        public List<string> Prerequisites { get; } = new();

        public bool HasSeatAvailable() => Enrolled < Capacity;

        public override string ToString() => $"{Code} - {Title} (Seats: {Enrolled}/{Capacity})";
    }
}