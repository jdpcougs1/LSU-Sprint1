using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSU_App
{
    /// <summary>
    /// Represents a faculty member within the LSU_App system.
    /// Contains identifying information and department assignment.
    /// </summary>
    public class Faculty
    {
        /// <summary>
        /// Unique identifier for the faculty member.
        /// Used as the primary key for database storage and for linking faculty actions
        /// to specific individuals in audit trails and application review processes.
        /// This ID may also correspond to employee numbers or other institutional identifiers.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the faculty member.
        /// Used for personalized communications, display in administrative interfaces,
        /// and official documentation when faculty provide input on applications.
        /// Defaults to empty string to prevent null reference exceptions during object initialization.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the faculty member.
        /// Used for alphabetical sorting of faculty lists, name-based searches,
        /// and formal identification in application review workflows.
        /// Critical for organizing faculty by surname in administrative displays.
        /// Defaults to empty string to prevent null reference exceptions during object initialization.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Department or academic unit to which the faculty member belongs.
        /// Examples: "Computer Science", "Mathematics", "English", "Business Administration"
        /// 
        /// This is the most critical property for access control - faculty members typically
        /// can only review applications for their own department or related programs.
        /// Used to filter which applications appear in a faculty member's review queue.
        /// 
        /// In a full implementation, this might be:
        /// - A foreign key to a Department entity
        /// - An enum of predefined departments
        /// - Part of a more complex organizational hierarchy
        /// 
        /// Defaults to empty string to prevent null reference exceptions during object initialization.
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Default parameterless constructor.
        /// Required for object-relational mapping (ORM) frameworks like Entity Framework,
        /// JSON serialization/deserialization for API endpoints, and other reflection-based operations.
        /// Creates a faculty member with default values (empty strings and Id = 0).
        /// In production use, prefer the parameterized constructor to ensure all required data is provided.
        /// </summary>
        public Faculty() { }

        /// <summary>
        /// Parameterized constructor for creating a fully-initialized Faculty object.
        /// Used when importing faculty data from external systems, creating test data,
        /// or when all faculty information is available at object creation time.
        /// Ensures that all essential faculty data is provided, reducing the risk of
        /// incomplete faculty records that could affect access control decisions.
        /// </summary>
        /// <param name="id">Unique identifier for the faculty member</param>
        /// <param name="firstName">Faculty member's first name</param>
        /// <param name="lastName">Faculty member's last name</param>
        /// <param name="department">Department or academic unit assignment</param>
        public Faculty(int id, string firstName, string lastName, string department)
        {
            // Initialize all properties with provided values
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Department = department;
        }

        /// <summary>
        /// Provides a human-readable string representation of the Faculty object.
        /// Used for debugging, logging, dropdown lists in administrative interfaces,
        /// and anywhere a quick faculty summary is needed.
        /// 
        /// The format includes the department information which is crucial for identifying
        /// the faculty member's area of expertise and access permissions.
        /// Format: "FirstName LastName (ID: #, Dept: DepartmentName)"
        /// </summary>
        /// <returns>Formatted string containing the faculty member's key identifying information</returns>
        /// <example>Returns: "John Smith (ID: 42, Dept: Computer Science)"</example>
        public override string ToString()
        {
            return $"{FirstName} {LastName} (ID: {Id}, Dept: {Department})";
        }
    }
}
