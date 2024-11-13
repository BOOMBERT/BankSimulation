using BankSimulation.Application.Common.Exceptions;
using FluentValidation.Results;

namespace BankSimulation.Application.Common.Utils
{
    internal static class ValidationUtils
    {
        internal static void CheckValidationResult(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new ValidationErrorException(errors);
            }
        }
    }
}
