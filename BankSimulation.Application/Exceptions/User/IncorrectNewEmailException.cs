using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.User
{
    public sealed class IncorrectNewEmailException : CustomException
    {
        public IncorrectNewEmailException(
            string errorContext,
            string title = "Incorrect Email",
            string details = "The provided new email is incorrect.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
