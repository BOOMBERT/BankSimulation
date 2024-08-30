using BankSimulation.Application.Dtos;
using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminBankAccountController : ControllerBase
    {
        private readonly IAdminBankAccountService _adminBankAccountService;

        public AdminBankAccountController(IAdminBankAccountService adminBankAccountService)
        {
            _adminBankAccountService = adminBankAccountService ?? throw new ArgumentNullException(nameof(adminBankAccountService));
        }

        [HttpPost("{userId}/bank-accounts"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> OpenBankAccount(Guid userId, Currency bankAccountCurrency)
        {
            return Ok(await _adminBankAccountService.CreateUserBankAccountAsync(userId, bankAccountCurrency));
        }

        [HttpDelete("bank-accounts/{accountNumber}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CloseBankAccount(string accountNumber)
        {
            return Ok(await _adminBankAccountService.DeleteUserBankAccountByNumberAsync(accountNumber));
        }

        [HttpGet("bank-accounts/{accountNumber}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BankAccountDto>> GetSpecificBankAccount(string accountNumber)
        {
            return Ok(await _adminBankAccountService.GetUserBankAccountByNumberAsync(accountNumber));
        }

        [HttpGet("{userId}/bank-accounts"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetUserAllBankAccounts(Guid userId)
        {
            return Ok(await _adminBankAccountService.GetAllBankAccountsByUserIdAsync(userId));
        }
    }
}
