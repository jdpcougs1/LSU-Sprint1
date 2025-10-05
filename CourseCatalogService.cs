using System;
using System.Collections.Generic;
using System.Linq;

namespace LSU_App
{
    /// Modified 03OCT25 Karima
    /// <summary>
    /// Course Catalog CRUD, admin 
    /// </summary>
    public sealed class CourseCatalogService
    {
        private readonly Dictionary<string, Course> _courses = new(StringComparer.OrdinalIgnoreCase);

        public CourseCatalogService()
        {
            // Sprint 2 / seed
            AddOrUpdate(new Course { Code = "CSCI-101", Title = "Intro to Programming", Department = "CS", Capacity = 40 });
            AddOrUpdate(new Course { Code = "CSCI-201", Title = "Data Structures", Department = "CS", Capacity = 35, Prerequisites = { "CSCI-101" } });
            AddOrUpdate(new Course { Code = "MATH-121", Title = "Calculus I", Department = "Math", Capacity = 40 });
        }

        public IEnumerable<Course> List() => _courses.Values.OrderBy(c => c.Code);

        public Course? Get(string code) => _courses.TryGetValue(code, out var c) ? c : null;

        public void AddOrUpdate(Course course)
        {
            if (string.IsNullOrWhiteSpace(course.Code))
                throw new ArgumentException("Course code is required.", nameof(course));
            _courses[course.Code] = course;
        }

        public bool Delete(string code) => _courses.Remove(code);

        public IEnumerable<Course> Search(string? term)
        {
            if (string.IsNullOrWhiteSpace(term)) return List();
            term = term.Trim();
            return _courses.Values.Where(c =>
                c.Code.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                c.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                c.Department.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
    }
}