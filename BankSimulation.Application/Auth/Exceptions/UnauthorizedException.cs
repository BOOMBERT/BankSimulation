using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Auth.Exceptions
{
    public sealed class UnauthorizedException : CustomException
    {
        public UnauthorizedException(
            string? errorContext = null,
            string title = "Unauthorized Access",
            string details = "You are not authorized to access this resource.")
            : base(title, StatusCodes.Status401Unauthorized, details, errorContext) { }
    }
}
