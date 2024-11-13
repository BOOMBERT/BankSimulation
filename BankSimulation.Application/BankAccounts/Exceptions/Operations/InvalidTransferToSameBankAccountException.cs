using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.BankAccounts.Exceptions.Operations
{
    public sealed class InvalidTransferToSameBankAccountException : CustomException
    {
        public InvalidTransferToSameBankAccountException(
            string errorContext,
            string title = "Invalid Transfer Attempt",
            string details = "Transfers between the same account are not allowed.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
