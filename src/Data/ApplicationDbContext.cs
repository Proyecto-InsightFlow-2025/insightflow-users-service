
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Data
{
    /// <summary>
    /// In-memory database context for the InsightFlow Users Service.
    /// </summary>
    /// <remarks>
    /// This class simulates a database context using in-memory collections.
    /// It is registered as a Singleton in the dependency injection container
    /// to maintain data persistence across HTTP requests during the application's lifetime.
    /// 
    /// Note: This is a simplified implementation for workshop/testing purposes.
    /// In a production environment, consider using Entity Framework Core with
    /// a proper database (SQL Server, PostgreSQL, etc.) for data persistence.
    /// 
    /// The context automatically seeds initial data on initialization to provide
    /// sample users for testing and demonstration.
    /// </remarks>
    public class ApplicationDbContext
    {
        /// <summary>
        /// Gets or sets the collection of users in the in-memory database.
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> of <see cref="User"/> entities representing
        /// all users stored in the system.
        /// </value>
        /// <remarks>
        /// This property serves as the main data store for user entities.
        /// All CRUD operations on users are performed against this collection.
        /// 
        /// Access to this collection should be managed through the repository layer
        /// to maintain separation of concerns and proper data access patterns.
        /// </remarks>
        public List<User> Users { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor:
        /// 1. Initializes an empty Users collection
        /// 2. Automatically seeds the database with sample data
        /// 
        /// Since this is a Singleton service, the constructor and seed method
        /// are called only once when the application starts.
        /// </remarks>
        public ApplicationDbContext()
        {
            Users = new List<User>();
            SeedData();
        }

        /// <summary>
        /// Seeds the in-memory database with initial sample data.
        /// </summary>
        /// <remarks>
        /// This method populates the Users collection with predefined sample users
        /// if the collection is currently empty. This ensures the application has
        /// test data available immediately after startup.
        /// 
        /// The seeding process:
        /// 1. Checks if the Users collection already contains data
        /// 2. Adds a set of predefined user entities (admin and regular users)
        /// 3. Uses BCrypt to hash passwords for security
        /// 
        /// In a production environment, seeding would typically be controlled
        /// via configuration and separated from the context initialization.
        /// </remarks>
        private void SeedData()
        {
            if (Users.Any()) return;

            Users.AddRange(new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@insightflow.cl",
                    Username = "admin_if",
                    Birthdate = new DateOnly(1990, 1, 1),
                    Address = "Calle Falsa 123",
                    PhoneNumber = "+56912345678",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("SecurePassword123!"),
                    IsActive = true,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    Role = 1  // Assuming admin has role 1
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Ignacio",
                    LastName = "Avendaño",
                    Email = "ignacio@insightflow.cl",
                    Username = "iavendano",
                    Birthdate = new DateOnly(1995, 5, 20),
                    Address = "Av. Brasil 123",
                    PhoneNumber = "+56987654321",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    IsActive = true,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    Role = 0
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "David",
                    LastName = "Araya",
                    Email = "david@insightflow.cl",
                    Username = "daraya",
                    Birthdate = new DateOnly(1985, 3, 15),
                    Address = "Angamos 0610",
                    PhoneNumber = "+56911223344",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    IsActive = true,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    Role = 0
                }
            });
        }

    }
}