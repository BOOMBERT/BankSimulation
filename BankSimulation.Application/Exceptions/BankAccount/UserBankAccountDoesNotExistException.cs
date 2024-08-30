
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount
{
    public sealed class UserBankAccountDoesNotExistException : CustomException
    {
        public UserBankAccountDoesNotExistException(
            string errorContext,
            string title = "User Bank Account Not Found",
            string details = "The specified user bank account does not exist or could not be found.")
            : base(title, StatusCodes.Status404NotFound, details, errorContext) { }
    }
}
