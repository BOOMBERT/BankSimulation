using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Users.Exceptions
{
    public sealed class UserNotFoundException : CustomException
    {
        public UserNotFoundException(
            string errorContext,
            string title = "User Not Found",
            string details = "The specified user could not be found.")
            : base(title, StatusCodes.Status404NotFound, details, errorContext) { }
    }
}
