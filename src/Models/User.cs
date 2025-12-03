using System.ComponentModel.DataAnnotations;

namespace insightflow_users_service.src.Models
{
    public class User
    {

        /// <summary>
        /// The User's UUID V4 identifier
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The User's name
        /// </summary> 
        [Required, MaxLength(20)]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The User's last names
        /// </summary> 
        [Required, MaxLength(20)]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// The User's email (@insightflow.cl)
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The User's username
        /// </summary>
        [Required, MaxLength(20)]
        public string Username { get; set; } = null!;

        /// <summary>
        /// The User's date of birth (YYYY-MM-DD) and over 18 years old
        /// </summary>
        [Required]
        public DateOnly Birthdate { get; set; }

        /// <summary>
        /// The User's address
        /// </summary>
        [Required]
        public string Address { get; set; } = null!;

        /// <summary>
        /// The User's phone number
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// The User's bcrypt encrypted password 
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Soft delete flag
        /// </summary>

        public bool? IsActive { get; set; } = true;

        /// <summary>
        /// User registration date
        /// </summary>
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        /// <summary>
        /// The User's role will primarily be 0, every 1 role is a controlled case
        /// </summary>
        public int Role { get; set; } = 0;
    }
}