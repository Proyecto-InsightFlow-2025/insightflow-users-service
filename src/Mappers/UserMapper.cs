
using insightflow_users_service.src.DTOs;
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Mappers
{
    /// <summary>
    /// Provides static methods for mapping between User entities and data transfer objects.
    /// </summary>
    /// <remarks>
    /// This mapper class handles bidirectional conversions between the User model
    /// and various DTOs used in the API, ensuring consistent data transformation
    /// and business rule enforcement across the application.
    /// </remarks>
    public static class UserMapper
    {
        /// <summary>
        /// Converts a CreateUserRequest DTO to a User entity.
        /// </summary>
        /// <param name="dto">The CreateUserRequest data transfer object containing user registration data.</param>
        /// <returns>A new User entity populated with data from the DTO.</returns>
        /// <remarks>
        /// <para>
        /// This method performs the following transformations:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>Generates a new GUID for the User ID</description>
        /// </item>
        /// <item>
        /// <description>Maps FirstName and LastName properties directly</description>
        /// </item>
        /// <item>
        /// <description>Hashes the password using BCrypt before storage</description>
        /// </item>
        /// <item>
        /// <description>Sets default values: Role = 0 (regular user), IsActive = true</description>
        /// </item>
        /// <item>
        /// <description>Sets CreatedAt to current UTC date</description>
        /// </item>
        /// </list>
        /// <example>
        /// The following example shows how to use this method:
        /// <code>
        /// var createRequest = new CreateUserRequest { ... };
        /// var user = createRequest.User();
        /// </code>
        /// </example>
        /// </remarks>
        public static User ToUser(this CreateUserRequest dto)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastNames = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                Birthdate = dto.BirthDate,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = 0,
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };
        }

        /// <summary>
        /// Converts a User entity to a ViewUserResponse DTO.
        /// </summary>
        /// <param name="user">The User entity to convert.</param>
        /// <returns>A ViewUserResponse DTO containing User data suitable for API responses.</returns>
        /// <remarks>
        /// <para>
        /// This method performs the following transformations:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>Combines FirstName and LastNames into a FullName property</description>
        /// </item>
        /// <item>
        /// <description>Excludes sensitive data like PasswordHash</description>
        /// </item>
        /// <item>
        /// <description>Converts DateOnly Birthdate to DateOnly BirthDate in response</description>
        /// </item>
        /// <item>
        /// <description>Converts DateOnly CreatedAt to DateTime for API compatibility</description>
        /// </item>
        /// <item>
        /// <description>Handles null IsActive values by defaulting to true</description>
        /// </item>
        /// </list>
        /// <example>
        /// The following example shows how to use this method:
        /// <code>
        /// var user = await _repository.GetByIdAsync(id);
        /// var response = user.ToViewUserResponse();
        /// </code>
        /// </example>
        /// </remarks>
        public static ViewUserResponse ToViewUserResponse(this User user)
        {
            return new ViewUserResponse
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastNames}",
                Email = user.Email,
                Username = user.Username,
                IsActive = user.IsActive ?? true,
                BirthDate = user.Birthdate,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt.ToDateTime(new TimeOnly(0, 0))
            };
        }

        /// <summary>
        /// Updates an existing user entity with data from a CreateUserRequest DTO.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <param name="dto">The CreateUserRequest DTO containing updated data.</param>
        /// <remarks>
        /// <para>
        /// This method performs the following updates:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>Updates all basic user properties (FirstName, LastNames, Email, etc.)</description>
        /// </item>
        /// <item>
        /// <description>Conditionally updates the password hash only if a new password is provided</description>
        /// </item>
        /// <item>
        /// <description>Preserves existing values for Id, Role, IsActive, and CreatedAt</description>
        /// </item>
        /// </list>
        /// <note type="important">
        /// The password is only updated if the DTO contains a non-empty password value.
        /// This allows for partial updates without requiring password re-entry.
        /// </note>
        /// <example>
        /// The following example shows how to use this method:
        /// <code>
        /// var existingUser = await _repository.GetByIdAsync(id);
        /// existingUser.UpdateUserFromDto(updateRequest);
        /// await _repository.UpdateAsync(existingUser);
        /// </code>
        /// </example>
        /// </remarks>
        public static void UpdateUserFromDto(this User user, CreateUserRequest dto)
        {
            user.FirstName = dto.FirstName;
            user.LastNames = dto.LastName;
            user.Email = dto.Email;
            user.Username = dto.Username;
            user.Birthdate = dto.BirthDate;
            user.Address = dto.Address;
            user.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }
    }
}