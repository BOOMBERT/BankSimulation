using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.BankAccounts.Exceptions.Operations
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
