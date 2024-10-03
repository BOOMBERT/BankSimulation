using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount
{
    public sealed class BankAccountAlreadyDeletedException : CustomException
    {
        public BankAccountAlreadyDeletedException(
            string errorContext,
            string title = "Bank Account Already Deleted",
            string details = "The specified bank account has already been deleted.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
