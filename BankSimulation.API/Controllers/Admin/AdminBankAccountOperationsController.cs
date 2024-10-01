using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Admin
{
    [Route("api/admin/users/{userId}/bank-accounts/{number}")]
    [ApiController]
    public class AdminBankAccountOperationsController : ControllerBase
    {
        private readonly IAdminBankAccountOperationsService _adminBankAccountOperationsService;

        public AdminBankAccountOperationsController(IAdminBankAccountOperationsService adminBankAccountOperationsService)
        {
            _adminBankAccountOperationsService = adminBankAccountOperationsService ?? throw new ArgumentNullException(nameof(adminBankAccountOperationsService));
        }

        [HttpPost("deposit"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> DepositUserMoney(Guid userId, string number, decimal amount, Currency currency)
        {
            await _adminBankAccountOperationsService.DepositUserMoneyAsync(userId, number, amount, currency);
            return NoContent();
        }
    }
}
