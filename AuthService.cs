using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LSU_App
{
    /// <summary>
    /// Defines the roles available in the LSU application system.
    /// Used for role-based access control to determine what features and data each user type can access.
    /// Maps to different user personas with varying permission levels throughout the application.
    /// </summary>
    public enum Role {
        /// <summary>Student users - can view their own applications and submit new ones.</summary>
        Student,
        /// <summary>Faculty users - can view applications for their department and make recommendations.</summary>
        Faculty,
        /// <summary>Administrative users - full access to manage applications, users, and system settings.</summary>
        Admin
    }

    /// <summary>
    /// Represents a user account in the authentication system.
    /// Immutable record-like class that stores credentials and role information.
    /// Uses init-only properties to prevent modification after creation, enhancing security.
    /// </summary>
    public sealed class UserAccount
    {
        /// <summary>
        /// The unique username for this account.
        /// Used as the primary identifier for login and user lookups.
        /// Case-insensitive matching is performed during authentication.
        /// Init-only to prevent username changes after account creation.
        /// </summary>
        public string Username { get; init; } = "";
        
        /// <summary>
        /// SHA256 hash of the user's password.
        /// Never stores the plain text password for security purposes.
        /// Generated using the Passwords.Hash() method during account creation.
        /// Init-only to prevent tampering with stored credentials.
        /// NOTE: In production, this should include salt and use a proper password hashing algorithm like bcrypt or Argon2.
        /// </summary>
        public string PasswordHash { get; init; } = "";

        /// <summary>
        /// The role assigned to this user account.
        /// Determines what permissions and features this user has access to.
        /// Used throughout the application for authorization decisions.
        /// Init-only to prevent privilege escalation after account creation.
        /// </summary>
        public Role Role { get; init; }

        /// <summary>
        /// Provides a human-readable representation of the user account.
        /// Useful for debugging, logging, and administrative displays.
        /// Format: "username (Role)"
        /// Does not expose sensitive information like password hashes.
        /// </summary>
        /// <returns>Formatted string showing username and role</returns>
        /// <example>Returns: "jsmith (Admin)" or "student123 (Student)"</example>
        public override string ToString() => $"{Username} ({Role})";
    }

    /// <summary>
    /// Static utility class for password hashing operations.
    /// Provides a centralized location for all password-related cryptographic functions.
    /// WARNING: This is a simplified implementation for Sprint 1 development only.
    /// </summary>
    public static class Passwords
    {
        /// <summary>
        /// Hashes a plain text password using SHA256.
        /// IMPORTANT: This is a demo implementation without salt - NOT suitable for production use!
        /// 
        /// Security limitations of this implementation:
        /// - No salt means identical passwords produce identical hashes (rainbow table vulnerability)
        /// - SHA256 is fast, making it vulnerable to brute force attacks
        /// - No key stretching or computational cost to slow down attackers
        /// 
        /// For production applications, use:
        /// - ASP.NET Core Identity for complete authentication/authorization
        /// - bcrypt, scrypt, or Argon2 for password hashing
        /// - Unique salt per password
        /// - Configurable work factors
        /// </summary>
        /// <param name="plain">The plain text password to hash</param>
        /// <returns>Hexadecimal string representation of the SHA256 hash</returns>
        public static string Hash(string plain)
        {
            // Create SHA256 hasher instance
            using var sha = SHA256.Create();

            // Convert password to bytes and compute hash
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));

            // Convert hash bytes to uppercase hexadecimal string
            return Convert.ToHexString(bytes);
        }
    }

    /// <summary>
    /// Minimal in-memory authentication service for Sprint 1 development.
    /// Provides basic user registration and login functionality without external dependencies.
    /// 
    /// PROTOTYPE LIMITATIONS:
    /// - All user data is stored in memory and lost on application restart
    /// - No password policies, account lockout, or security features
    /// - No audit logging of authentication events
    /// - Simple password hashing without salt (see Passwords class)
    /// - No session management or token-based authentication
    /// 
    /// For production use, replace with:
    /// - ASP.NET Core Identity with Entity Framework persistence
    /// - JWT tokens or cookie-based authentication
    /// - Proper password policies and security measures
    /// - Database storage with proper user management
    /// </summary>
    public sealed class AuthService
    {
        /// <summary>
        /// In-memory storage for all user accounts.
        /// Uses case-insensitive dictionary to allow flexible username matching.
        /// Key: username (case-insensitive), Value: UserAccount object
        /// In production, this would be replaced with database storage.
        /// </summary>
        private readonly Dictionary<string, UserAccount> _users =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Registers a new user account in the system.
        /// Automatically hashes the provided password and stores the account.
        /// Will overwrite existing usernames without warning (no duplicate checking).
        /// </summary>
        /// <param name="username">Unique username for the new account (case-insensitive)</param>
        /// <param name="password">Plain text password (will be hashed before storage)</param>
        /// <param name="role">Role to assign to this user (Student, Faculty, or Admin)</param>
        public void AddUser(string username, string password, Role role)
        {
            // Create new immutable user account with hashed password
            _users[username] = new UserAccount
            {
                Username = username,
                PasswordHash = Passwords.Hash(password), // Hash the password before storing
                Role = role
            };
        }

        /// <summary>
        /// Attempts to authenticate a user with username and password.
        /// Performs case-insensitive username lookup and secure password comparison.
        /// </summary>
        /// <param name="username">Username to authenticate (case-insensitive)</param>
        /// <param name="password">Plain text password to verify</param>
        /// <returns>
        /// UserAccount object if authentication successful; 
        /// null if username not found or password incorrect
        /// </returns>
        public UserAccount? Login(string username, string password)
        {
            // Try to find the user account (case-insensitive lookup)
            if (!_users.TryGetValue(username, out var u)) return null; // User not found

            // Compare hashed password with stored hash
            // Hash the provided password and compare with stored hash
            return u.PasswordHash == Passwords.Hash(password) ? u : null;
        }
    }
}
