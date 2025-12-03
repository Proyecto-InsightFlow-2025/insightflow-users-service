
using insightflow_users_service.src.Data;
using insightflow_users_service.src.Helpers.Requests;
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Repositories
{
    /// <summary>
    /// Repository implementation for managing User entities in-memory.
    /// Provides CRUD operations and query capabilities for User data.
    /// </summary>
    /// <remarks>
    /// This repository uses an in-memory data store (List) and follows
    /// the repository pattern to abstract data access logic.
    /// </remarks>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The in-memory database context containing the Users collection.</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new user in the in-memory data store.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>A task that represents the asynchronous operation, containing the created user.</returns>
        /// <remarks>
        /// This method adds the user to the in-memory list. Since we're using a List,
        /// no explicit SaveChangesAsync call is needed.
        /// </remarks>
        public Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Retrieves users with filtering, sorting, and pagination support.
        /// </summary>
        /// <param name="query">The query parameters containing filters, sorting options, and pagination settings.</param>
        /// <returns>A task that represents the asynchronous operation, containing a tuple with the filtered users and total count.</returns>
        /// <remarks>
        /// This method supports:
        /// - Filtering by: FirstName, LastName, Email, Username, IsActive
        /// - Sorting by: FirstName, LastName, Email, Username, CreatedAt
        /// - Pagination with page number and page size
        /// </remarks>
        public Task<(IEnumerable<User> Users, int TotalCount)> GetAllAsync(UserQuery query)
        {
            var usersQuery = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                usersQuery = usersQuery.Where(u => u.FirstName.Contains(query.FirstName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                usersQuery = usersQuery.Where(u => u.LastName.Contains(query.LastName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(query.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                usersQuery = usersQuery.Where(u => u.Username.Contains(query.Username, StringComparison.OrdinalIgnoreCase));
            }

            if (query.IsActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == query.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                usersQuery = query.SortBy switch
                {
                    "FirstName" => query.IsDescending ? usersQuery.OrderByDescending(u => u.FirstName) : usersQuery.OrderBy(u => u.FirstName),
                    "LastName" => query.IsDescending ? usersQuery.OrderByDescending(u => u.LastName) : usersQuery.OrderBy(u => u.LastName),
                    "Email" => query.IsDescending ? usersQuery.OrderByDescending(u => u.Email) : usersQuery.OrderBy(u => u.Email),
                    "Username" => query.IsDescending ? usersQuery.OrderByDescending(u => u.Username) : usersQuery.OrderBy(u => u.Username),
                    "CreatedAt" => query.IsDescending ? usersQuery.OrderByDescending(u => u.CreatedAt) : usersQuery.OrderBy(u => u.CreatedAt),
                    _ => usersQuery
                };
            }

            var totalCount = usersQuery.Count();
            var users = usersQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return Task.FromResult((users.AsEnumerable(), totalCount));
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The GUID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found; otherwise, null.</returns>
        public Task<User?> GetByIdAsync(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Retrieves a user by their email address (case-insensitive).
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found; otherwise, null.</returns>
        /// <remarks>
        /// This method is commonly used for authentication and password reset operations.
        /// </remarks>
        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        /// <summary>
        /// Retrieves a user by their username (case-insensitive).
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found; otherwise, null.</returns>
        public Task<User?> GetByUsernameAsync(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">The user entity with updated values.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        /// <remarks>
        /// This method finds the existing user by ID and updates all properties except:
        /// - Password (should be updated separately with hashing)
        /// - ID (never updated)
        /// - CreatedAt (should remain unchanged)
        /// 
        /// Note: Since this is an in-memory implementation, we need to manually
        /// copy properties from the incoming object to the existing reference.
        /// </remarks>
        public Task UpdateAsync(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Username = user.Username;
                existingUser.Email = user.Email; 
                existingUser.Birthdate = user.Birthdate;
                existingUser.Address = user.Address;
                existingUser.PhoneNumber = user.PhoneNumber;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Performs a soft delete on a user by setting their IsActive flag to false.
        /// </summary>
        /// <param name="id">The GUID of the user to soft delete.</param>
        /// <returns>A task that represents the asynchronous soft delete operation.</returns>
        /// <remarks>
        /// This method implements soft deletion rather than physical deletion,
        /// allowing for potential data recovery and maintaining referential integrity.
        /// </remarks>
        public Task SoftDeleteAsync(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsActive = false;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if a user with the specified email already exists in the data store.
        /// </summary>
        /// <param name="email">The email address to check (case-insensitive).</param>
        /// <returns>A task that represents the asynchronous operation, containing true if a user with the email exists; otherwise, false.</returns>
        /// <remarks>
        /// This method is used for validation during user registration and updates
        /// to ensure email uniqueness.
        /// </remarks>
        public Task<bool> ExistsByEmailAsync(string email)
        {
            var exists = _context.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        /// <summary>
        /// Checks if a user with the specified username already exists in the data store.
        /// </summary>
        /// <param name="username">The username to check (case-insensitive).</param>
        /// <returns>A task that represents the asynchronous operation, containing true if a user with the username exists; otherwise, false.</returns>
        /// <remarks>
        /// This method is used for validation during user registration and updates
        /// to ensure username uniqueness.
        /// </remarks>
        public Task<bool> ExistsByUsernameAsync(string username)
        {
            var exists = _context.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }
    }

}