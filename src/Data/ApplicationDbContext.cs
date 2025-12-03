using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Data
{
    public class ApplicationDbContext
    {
        // The "Database" in memory
        public List<User> Users { get; set; }

        public ApplicationDbContext()
        {
            Users = new List<User>();
            SeedData();
        }

        // Rubric Requirement: "Seeder de usuarios"
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