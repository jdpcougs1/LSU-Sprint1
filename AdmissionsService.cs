using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSU_App
{
    public enum AppStatus {
        /// <summary>Application has been submitted but not yet reviewed.</summary>
        Submitted,
        /// <summary>Application has been reviewed and accepted for admission.</summary>
        Accepted,
        /// <summary>Application has been reviewed and rejected for admission.</summary>
        Rejected
    }

    /// <summary>
    /// Simple in-memory admissions service for Sprint 1.
    /// Manages the complete lifecycle of student applications from submission to decision.
    /// NOTE: This is a prototype implementation - data is not persisted and will be lost on restart.
    /// </summary>
    public sealed class AdmissionsService
    {
        /// <summary>
        /// Auto-incrementing counter to ensure each applicant gets a unique identifier.
        /// Starts at 1 and increments with each new submission.
        /// </summary>
        private int _nextId = 1;
        /// <summary>
        /// In-memory storage for all submitted applications.
        /// Uses a List for simple sequential access and modification.
        /// In production, this would be replaced with database storage.
        /// </summary>
        private readonly List<Applicant> _apps = new();

        /// <summary>
        /// Processes a new student application submission.
        /// Creates an applicant record with a unique ID and initial "Submitted" status.
        /// </summary>
        /// <param name="first">Applicant's first name</param>
        /// <param name="last">Applicant's last name</param>
        /// <param name="submittedOn">Date and time when the application was submitted</param>
        /// <returns>The unique ID assigned to this application for future reference</returns>
        public int Submit(string first, string last, DateTime submittedOn)
        {
            // Create new applicant with auto-generated ID and default "Submitted" status
            var a = new Applicant(_nextId++, first, last, submittedOn, AppStatus.Submitted.ToString());
            // add to in-memory list and return the ID number
            _apps.Add(a);
            return a.Id;
        }

        /// <summary>
        /// Retrieves all submitted applications regardless of status.
        /// Returns a read-only view to prevent external modification of the internal collection.
        /// </summary>
        /// <returns>Read-only list of all applicants in the system</returns>
        public IReadOnlyList<Applicant> ListAll() => _apps;

        /// <summary>
        /// Searches for all applicants with a specific last name.
        /// Performs case-insensitive comparison to handle different capitalizations.
        /// </summary>
        /// <param name="last">Last name to search for (case-insensitive)</param>
        /// <returns>All applicants matching the specified last name</returns>
        public IEnumerable<Applicant> ListByLastName(string last) =>
            _apps.Where(a => a.LastName.Equals(last, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Updates an application's status after admissions review.
        /// Allows changing from Submitted to either Accepted or Rejected.
        /// </summary>
        /// <param name="id">Unique identifier of the application to update</param>
        /// <param name="newStatus">New status to assign (Accepted or Rejected typically)</param>
        /// <returns>
        /// True if the application was found and status updated successfully;
        /// False if no application exists with the specified ID
        /// </returns>
        public bool Decide(int id, AppStatus newStatus)
        {
            // Search for the application by ID
            // If application doesn't exist, return failure
            var a = _apps.FirstOrDefault(x => x.Id == id);
            if (a is null) return false;
            // Update the status and return success
            a.Status = newStatus.ToString();
            return true;
        }
    }
}
