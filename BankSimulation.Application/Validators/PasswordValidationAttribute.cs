using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BankSimulation.Application.Validators
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public PasswordValidationAttribute(int minLength, int maxLength)
        {
             _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required.");
            }

            if (password.Length < _minLength || password.Length > _maxLength)
            {
                return new ValidationResult($"Password must have a minimum of {_minLength} characters and a maximum of {_maxLength} characters.");
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Password must contain at least one digit.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ValidationResult("Password must contain at least one lowercase letter.");
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("Password must contain at least one non-alphanumeric character.");
            }

            return ValidationResult.Success!;
        }
    }
}
