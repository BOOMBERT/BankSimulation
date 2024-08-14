using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions
{
    public sealed class ValidationErrorException : CustomException
    {
        public ValidationErrorException(
            object details,
            string? errorContext = null,
            string title = "One or more validation errors occurred.")
           : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
