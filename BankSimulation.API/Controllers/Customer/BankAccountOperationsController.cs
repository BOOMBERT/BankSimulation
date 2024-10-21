using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Customer
{
    [Route("api/users/bank-accounts/{bankAccountNumber}")]
    [ApiController]
    public class BankAccountOperationsController : ControllerBase
    {
        private readonly IBankAccountOperationsService _bankAccountOperationsService;

        public BankAccountOperationsController(IBankAccountOperationsService bankAccountOperationsService)
        {
            _bankAccountOperationsService = bankAccountOperationsService ?? throw new ArgumentNullException(nameof(bankAccountOperationsService));
        }

        [HttpPost("transfer/{recipientBankAccountNumber}"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> TransferMoneyFromOwnBankAccount(string bankAccountNumber, string recipientBankAccountNumber, decimal amount)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            await _bankAccountOperationsService.TransferMoneyAsync(accessTokenFromHeader, bankAccountNumber, recipientBankAccountNumber, amount);
            return NoContent();
        }
    }
}
