

namespace insightflow_users_service.src.Helpers.Requests
{
    /// <summary>
    /// Represents query parameters for filtering, sorting, and paginating client records.
    /// </summary>
    /// <remarks>
    /// This class is used to encapsulate all possible query parameters when retrieving
    /// a list of users from the API. It supports filtering, sorting, and pagination.
    /// </remarks>
    public class UserQuery
    {
        /// <summary>
        /// Gets or sets the first name to filter users by.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last names to filter users by.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address to filter users by.
        /// </summary>
        /// <value>
        /// A string containing the exact email address or part of it to search for.
        /// If null or empty, no filtering by email is applied.
        /// </value>
        /// <remarks>
        /// This filter performs a case-insensitive partial match on the client's email address.
        /// </remarks>
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username to filter users by.
        /// </summary>
        public string? Username { get; set; }
        
        /// <summary>
        /// Gets or sets the active status to filter users by.
        /// </summary>
        /// <value>
        /// <c>true</c> to filter only active users; <c>false</c> to filter only inactive users;
        /// <c>null</c> to include both active and inactive users (no filtering by status).
        /// </value>
        public bool? IsActive { get; set; } = null;

        /// <summary>
        /// Gets or sets the property name to sort the results by.
        /// </summary>
        /// <value>
        /// The name of the client property to sort by ("FirstName", "", "Email", "Username" or "CreatedAt").
        /// If null or empty, default sorting is applied.
        /// </value>
        /// <remarks>
        /// Supported sort fields typically include: FirstName, LastName, Email, Username or CreatedAt.
        /// </remarks>
        public string? SortBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the sort order is descending.
        /// </summary>
        /// <value>
        /// <c>true</c> for descending order (Z-A, newest first); <c>false</c> for ascending order (A-Z, oldest first).
        /// Default is <c>false</c> (ascending order).
        /// </value>
        public bool IsDescending { get; set; } = false;

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        /// <value>
        /// The one-based page number to retrieve. Default is 1 (first page).
        /// </value>
        /// <remarks>
        /// Page numbers start at 1. Values less than 1 will be treated as page 1.
        /// </remarks>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of records per page for pagination.
        /// </summary>
        /// <value>
        /// The maximum number of client records to return per page. Default is 10.
        /// </value>
        /// <remarks>
        /// Typical values range from 10 to 100. The actual number of records returned
        /// may be less than this value if there are insufficient records on the requested page.
        /// </remarks>
        public int PageSize { get; set; } = 10;
    }

}