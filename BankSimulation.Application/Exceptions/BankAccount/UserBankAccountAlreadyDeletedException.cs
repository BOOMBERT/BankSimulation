using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount
{
    public sealed class UserBankAccountAlreadyDeletedException : CustomException
    {
        public UserBankAccountAlreadyDeletedException(
            string errorContext,
            string title = "User Bank Account Already Deleted",
            string details = "The specified user bank account has already been deleted.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
