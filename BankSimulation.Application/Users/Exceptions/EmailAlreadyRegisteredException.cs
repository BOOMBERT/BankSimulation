using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Users.Exceptions
{
    public sealed class EmailAlreadyRegisteredException : CustomException
    {
        public EmailAlreadyRegisteredException(
            string errorContext,
            string title = "Incorrect Email",
            string details = "This email address is already registered.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
