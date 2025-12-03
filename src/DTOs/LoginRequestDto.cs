
namespace insightflow_users_service.src.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for user authentication requests.
    /// </summary>
    /// <remarks>
    /// This DTO is used to transfer authentication credentials from the client to the server
    /// during the login process. It contains the minimum required information to verify
    /// a user's identity.
    /// 
    /// Security Note: The password is transmitted in plain text over HTTPS. In production,
    /// consider additional security measures such as implementing token-based authentication
    /// with proper encryption for sensitive applications.
    /// </remarks>
    public class LoginRequestDto
    {
        
        /// <summary>
        /// Gets or sets the user's email address used for authentication.
        /// </summary>
        /// <value>
        /// A string containing the email address associated with the user account.
        /// The email must be valid and registered in the system for successful authentication.
        /// </value>
        /// <remarks>
        /// Email addresses are treated as case-insensitive during authentication.
        /// This field is required for all login attempts.
        /// </remarks>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user's password for authentication.
        /// </summary>
        /// <value>
        /// A string containing the plain-text password provided by the user.
        /// This password will be hashed and compared against the stored hash in the database.
        /// </value>
        /// <remarks>
        /// For security reasons:
        /// - Passwords should be at least 8 characters long
        /// - Passwords should contain a mix of uppercase, lowercase, numbers, and special characters
        /// - The plain password is never stored; only its hash is persisted
        /// - This field is required for all login attempts
        /// </remarks>
        public string Password { get; set; } = string.Empty;
        
    }
}