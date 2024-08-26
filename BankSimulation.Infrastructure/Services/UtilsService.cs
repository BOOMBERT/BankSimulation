using BankSimulation.Application.Exceptions;
using FluentValidation.Results;

namespace BankSimulation.Infrastructure.Services
{
    internal static class UtilsService
    {
        public static void CheckValidationResult(ValidationResult validationResult)
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
