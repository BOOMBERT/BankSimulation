using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Users.Exceptions
{
    public sealed class UserAlreadyDeletedException : CustomException
    {
        public UserAlreadyDeletedException(
            string errorContext,
            string title = "User Already Deleted",
            string details = "The specified user has already been deleted.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
