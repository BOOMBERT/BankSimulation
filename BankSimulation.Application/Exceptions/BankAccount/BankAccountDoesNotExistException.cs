using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount
{
    public sealed class BankAccountDoesNotExistException : CustomException
    {
        public BankAccountDoesNotExistException(
            string errorContext,
            string title = "Bank Account Not Found",
            string details = "The specified bank account does not exist or could not be found.")
            : base(title, StatusCodes.Status404NotFound, details, errorContext) { }
    }
}
