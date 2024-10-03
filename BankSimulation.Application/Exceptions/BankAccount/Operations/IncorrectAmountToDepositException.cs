using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount.Operations
{
    public sealed class IncorrectAmountToDepositException : CustomException
    {
        public IncorrectAmountToDepositException(
            string errorContext,
            string title = "Incorrect Amount to Deposit",
            string details = "The amount specified for deposit must be a positive value.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
