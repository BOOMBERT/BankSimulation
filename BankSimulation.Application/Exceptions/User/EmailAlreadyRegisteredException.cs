using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.User
{
    public sealed class EmailAlreadyRegisteredException : CustomException
    {
        public EmailAlreadyRegisteredException(
            string errorContext,
            string title = "Email Registration Error",
            string details = "This email address is already registered.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
