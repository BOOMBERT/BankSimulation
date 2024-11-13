using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.BankAccounts.Exceptions
{
    public sealed class BankAccountBalanceTooLowException : CustomException
    {
        public BankAccountBalanceTooLowException(
            string errorContext,
            string title = "Bank Account Balance Too Low",
            string details = "The bank account balance is too low to complete this operation.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
