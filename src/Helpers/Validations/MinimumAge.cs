
using System.ComponentModel.DataAnnotations;

namespace insightflow_users_service.src.Helpers.Validation
{
    /// <summary>
    /// Validates that a date represents a person who is at least a specified minimum age.
    /// </summary>
    /// <remarks>
    /// This custom validation attribute ensures that a DateOnly value represents a birth date
    /// where the person is at least the specified minimum age as of the current UTC date.
    /// </remarks>
    public class MinimumAge : ValidationAttribute
    {
        private readonly int _minAge;

        /// <summary>
        /// Initializes a new instance of the MinimumAge validation attribute.
        /// </summary>
        /// <param name="minAge">The minimum required age in years.</param>
        /// <remarks>
        /// The validation will pass if the person's age is greater than or equal to the specified minimum age.
        /// </remarks>
        /// <example>
        /// The following example shows how to apply the MinimumAge attribute:
        /// <code>
        /// [MinimumAge(18)]
        /// public DateOnly BirthDate { get; set; }
        /// </code>
        /// </example>
        public MinimumAge(int minAge)
        {
            _minAge = minAge;
            ErrorMessage = $"Cliente debe tener al menos {minAge} años.";
        }

        /// <summary>
        /// Validates whether the specified value meets the minimum age requirement.
        /// </summary>
        /// <param name="value">The value to validate. Expected to be a DateOnly representing a birth date.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// <see cref="ValidationResult.Success"/> if the value is valid; 
        /// otherwise, a <see cref="ValidationResult"/> containing an error message.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The validation process:
        /// </para>
        /// <list type="number">
        /// <item>
        /// <description>If the value is null, validation passes (consider using [Required] for null checks)</description>
        /// </item>
        /// <item>
        /// <description>If the value is not a DateOnly, returns a format error</description>
        /// </item>
        /// <item>
        /// <description>Calculates age based on current UTC date and the birth date</description>
        /// </item>
        /// <item>
        /// <description>Returns success if age >= minimum age; otherwise returns error</description>
        /// </item>
        /// </list>
        /// <para>
        /// Age calculation accounts for whether the birthday has occurred yet in the current year.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the value is not a DateOnly type (though handled gracefully with error message).
        /// </exception>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            if (value is not DateOnly birthDate)
                return new ValidationResult("Formato de fecha inválido. Ingrese tipo de dato DateOnly.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;

            return age >= _minAge
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }

    }
}