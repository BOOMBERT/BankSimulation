using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Auth.Exceptions
{
    public sealed class ForbiddenAccessException : CustomException
    {
        public ForbiddenAccessException(
            string? errorContext = null,
            string title = "Forbidden Access",
            string details = "You do not have permission to access this resource.")
            : base(title, StatusCodes.Status403Forbidden, details, errorContext) { }
    }
}