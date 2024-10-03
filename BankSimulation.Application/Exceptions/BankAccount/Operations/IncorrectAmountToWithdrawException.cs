using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount.Operations
{
    public sealed class IncorrectAmountToWithdrawException : CustomException
    {
        public IncorrectAmountToWithdrawException(
            string errorContext,
            string title = "Incorrect Amount to Withdraw",
            string details = "The amount specified for withdraw must be a positive value.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
