
using System.ComponentModel.DataAnnotations;
using insightflow_users_service.src.Helpers.Validation;

namespace insightflow_users_service.src.DTOs
{
    public class UpdateUserRequest
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [EmailAddress]
        [RegularExpression(@"^[^@]+@insightflow\.cl$", ErrorMessage = "Email debe pertenecer a @insightflow.cl")]
        public string? Email { get; set; }

        [MinLength(4), MaxLength(50)]
        public string? Username { get; set; }

        [MinimumAge(18)]
        public DateOnly? BirthDate { get; set; }

        public string? Address { get; set; }

        [RegularExpression(@"^\+56[2-9]\d{8}$", ErrorMessage = "Número telefónico no válido. Formato esperado: +56911223344")]
        public string? PhoneNumber { get; set; }

        // Password is now OPTIONAL. 
        // If sent, we verify/update it. If null, we ignore it.
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
            ErrorMessage = "La contraseña debe incluir al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string? Password { get; set; }
        
    }
}