
using insightflow_users_service.src.Data;
using insightflow_users_service.src.Helpers.Requests;
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            // No SaveChangesAsync needed for List
            return Task.FromResult(user);
        }

        public Task<(IEnumerable<User> Users, int TotalCount)> GetAllAsync(UserQuery query)
        {
            // 1. Get AsQueryable to build the query dynamically
            var usersQuery = _context.Users.AsQueryable();

            // 2. Apply Filters
            if (!string.IsNullOrWhiteSpace(query.FullName))
            {
                usersQuery = usersQuery.Where(u => u.FullName.Contains(query.FullName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Equals(query.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                usersQuery = usersQuery.Where(u => u.Username.Contains(query.Username, StringComparison.OrdinalIgnoreCase));
            }

            if (query.IsActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == query.IsActive);
            }

            // 3. Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                usersQuery = query.SortBy switch
                {
                    "FullName" => query.IsDescending ? usersQuery.OrderByDescending(u => u.FullName) : usersQuery.OrderBy(u => u.FullName),
                    "Email" => query.IsDescending ? usersQuery.OrderByDescending(u => u.Email) : usersQuery.OrderBy(u => u.Email),
                    "Username" => query.IsDescending ? usersQuery.OrderByDescending(u => u.Username) : usersQuery.OrderBy(u => u.Username),
                    "CreatedAt" => query.IsDescending ? usersQuery.OrderByDescending(u => u.CreatedAt) : usersQuery.OrderBy(u => u.CreatedAt),
                    _ => usersQuery
                };
            }

            // 4. Total Count (Before Pagination)
            var totalCount = usersQuery.Count();

            // 5. Pagination
            var users = usersQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return Task.FromResult((users.AsEnumerable(), totalCount));
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task UpdateAsync(User user)
        {
            // In Memory, we are working with references. 
            // If the object passed here is the same reference from the list, it's already updated.
            // If it's a new object (from a PUT request), we need to find the old one and replace values.
            
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Username = user.Username;
                existingUser.Email = user.Email; // Ideally validate unique again before this
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.Address = user.Address;
                existingUser.PhoneNumber = user.PhoneNumber;
                // Do not update Password or ID here usually unless specific flow
            }

            return Task.CompletedTask;
        }

        public Task SoftDeleteAsync(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsActive = false;
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            var exists = _context.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task<bool> ExistsByUsernameAsync(string username)
        {
            var exists = _context.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }
    }

}