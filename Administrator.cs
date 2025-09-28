using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSU_App
{
    /// <summary>
    /// Represents an administrator within the LSU_App system.
    /// Contains identifying information and administrative role for access control and audit trails.
    /// This is a data model class that can be used for authentication, authorization, and tracking
    /// administrative actions throughout the application.
    /// </summary>
    public class Administrator
    {
        /// <summary>
        /// Unique identifier for the administrator.
        /// Used as the primary key for database storage and for referencing specific administrators
        /// in logs, audit trails, and administrative actions.
        /// </summary>
        public int Id { get; set; }

        /// First name of the administrator.
        /// Used for display purposes and personalization throughout the application interface.
        /// Defaults to empty string to prevent null reference exceptions.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the administrator.
        /// Used for display purposes and sorting administrator lists alphabetically.
        /// Defaults to empty string to prevent null reference exceptions.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Role or title of the administrator within the system.
        /// Examples: "System Admin", "Registrar", "Admissions Officer", "Department Head"
        /// Used for role-based access control to determine what actions this administrator can perform.
        /// Defaults to empty string to prevent null reference exceptions.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Default parameterless constructor.
        /// Required for object-relational mapping (ORM) frameworks like Entity Framework,
        /// JSON serialization/deserialization, and other reflection-based operations.
        /// Creates an administrator with default values (empty strings and Id = 0).
        /// </summary>
        public Administrator() { }

        /// <summary>
        /// Parameterized constructor for creating a fully-initialized Administrator object.
        /// Useful when creating new administrator records with all required information at once.
        /// </summary>
        /// <param name="id">Unique identifier for the administrator</param>
        /// <param name="firstName">Administrator's first name</param>
        /// <param name="lastName">Administrator's last name</param>
        /// <param name="role">Administrator's role or title in the system</param>
        public Administrator(int id, string firstName, string lastName, string role)
        {
            // Assign all properties from constructor parameters
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }

        /// <summary>
        /// Provides a human-readable string representation of the Administrator object.
        /// Useful for debugging, logging, dropdown displays, and anywhere a quick summary is needed.
        /// Format: "FirstName LastName (ID: #, Role: RoleName)"
        /// </summary>
        /// <returns>Formatted string containing the administrator's key information</returns>
        /// <example>Returns: "John Smith (ID: 42, Role: System Admin)"</example>
        public override string ToString()
        {
            return $"{FirstName} {LastName} (ID: {Id}, Role: {Role})";
        }
    }
}
