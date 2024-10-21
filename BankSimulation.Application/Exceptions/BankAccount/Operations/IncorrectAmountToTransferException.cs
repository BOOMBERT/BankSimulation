using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.BankAccount.Operations
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
