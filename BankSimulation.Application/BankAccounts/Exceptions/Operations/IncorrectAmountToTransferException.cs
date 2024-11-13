using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.BankAccounts.Exceptions.Operations
{
    public sealed class IncorrectAmountToTransferException : CustomException
    {
        public IncorrectAmountToTransferException(
            string errorContext,
            string title = "Incorrect Amount to Transfer",
            string details = "The amount specified for transfer must be a positive value.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
