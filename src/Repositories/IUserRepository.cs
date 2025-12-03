
using insightflow_users_service.src.Helpers.Requests;
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Repositories
{
    /// <summary>
    /// Defines the contract for user data access operations in the InsightFlow Users Service.
    /// </summary>
    /// <remarks>
    /// This interface follows the Repository Pattern to abstract data persistence details
    /// from the business logic layer. Implementations can use various data stores
    /// (in-memory, database, etc.) without affecting consumers of this interface.
    /// </remarks>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user in the data store.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>A task that represents the asynchronous operation, containing the created user.</returns>
        Task<User> CreateAsync(User user);
        
        /// <summary>
        /// Retrieves a paginated list of users with optional filtering, sorting, and pagination.
        /// </summary>
        /// <param name="query">The query parameters containing filters, sorting options, and pagination settings.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a tuple with:
        /// - The filtered and paginated list of users
        /// - The total count of users matching the filters (before pagination)
        /// </returns>
        Task<(IEnumerable<User> Users, int TotalCount)> GetAllAsync(UserQuery query);
        
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The GUID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found; otherwise, null.</returns>
        /// <remarks>
        /// This method is typically used for authentication and account recovery operations.
        /// </remarks>
        Task<User?> GetByEmailAsync(string email);
        
        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetByUsernameAsync(string username);
        
        /// <summary>
        /// Updates an existing user's information in the data store.
        /// </summary>
        /// <param name="user">The user entity with updated values.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateAsync(User user);
        
        /// <summary>
        /// Performs a soft delete on a user by marking them as inactive.
        /// </summary>
        /// <param name="id">The GUID of the user to soft delete.</param>
        /// <returns>A task that represents the asynchronous soft delete operation.</returns>
        /// <remarks>
        /// Soft deletion preserves the user record while preventing them from accessing the system.
        /// This is preferred over hard deletion for audit and data integrity purposes.
        /// </remarks>
        Task SoftDeleteAsync(Guid id);
        
        /// <summary>
        /// Checks whether a user with the specified email address already exists in the data store.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>A task that represents the asynchronous operation, containing true if a user with the email exists; otherwise, false.</returns>
        /// <remarks>
        /// This method is used for validation to ensure email uniqueness during user registration and updates.
        /// </remarks>
        Task<bool> ExistsByEmailAsync(string email);
        
        /// <summary>
        /// Checks whether a user with the specified username already exists in the data store.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>A task that represents the asynchronous operation, containing true if a user with the username exists; otherwise, false.</returns>
        /// <remarks>
        /// This method is used for validation to ensure username uniqueness during user registration and updates.
        /// </remarks>
        Task<bool> ExistsByUsernameAsync(string username);
    }
}