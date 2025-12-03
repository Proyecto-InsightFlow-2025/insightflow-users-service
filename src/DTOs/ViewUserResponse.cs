using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace insightflow_users_service.src.DTOs
{
    /// <summary>
    /// Represents a data transfer object for viewing client information in the insight flow system.
    /// </summary>
    /// <remarks>
    /// This DTO is used to return client data to API consumers, containing
    /// non-sensitive information suitable for display purposes.
    /// </remarks>
    public class ViewUserResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the client.
        /// </summary>
        /// <value>
        /// A GUID that uniquely identifies the client in the system.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the client.
        /// </summary>
        /// <value>
        /// The concatenated full name of the client, typically composed of first name and last names.
        /// </value>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the client's email address.
        /// </summary>
        /// <value>
        /// The email address associated with the client's account in the format username@insightflow.cl.
        /// </value>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the client's username.
        /// </summary>
        /// <value>
        /// The unique username chosen by the client for account identification and login purposes.
        /// </value>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the client account is active.
        /// </summary>
        /// <value>
        /// <c>true</c> if the client account is active and accessible; <c>false</c> if the account has been deactivated or soft-deleted.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the client's date of birth.
        /// </summary>
        /// <value>
        /// The birth date of the client in YYYY-MM-DD format.
        /// </value>
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the client's physical address.
        /// </summary>
        /// <value>
        /// The complete physical address where the client resides.
        /// </value>
        public string Address { get; set; } = null!;

        /// <summary>
        /// Gets or sets the client's phone number.
        /// </summary>
        /// <value>
        /// The client's contact phone number in Chilean format (+56 followed by 9 digits).
        /// </value>
        /// <remarks>
        /// Format: +56911223344
        /// </remarks>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the client account was created.
        /// </summary>
        /// <value>
        /// The UTC date and time when the client account was registered in the system.
        /// </value>
        public DateTime CreatedAt { get; set; }

    }

}