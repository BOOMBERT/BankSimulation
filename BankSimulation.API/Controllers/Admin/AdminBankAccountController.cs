﻿using BankSimulation.Application.Dtos.BankAccount;
using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Admin
{
    [Route("api/admin/users/{userId}/bank-accounts")]
    [ApiController]
    public class AdminBankAccountController : ControllerBase
    {
        private readonly IAdminBankAccountService _adminBankAccountService;

        public AdminBankAccountController(IAdminBankAccountService adminBankAccountService)
        {
            _adminBankAccountService = adminBankAccountService ?? throw new ArgumentNullException(nameof(adminBankAccountService));
        }

        [HttpPost, Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BankAccountDto>> OpenBankAccount(Guid userId, Currency currency)
        {
            var createdBankAccount = await _adminBankAccountService.CreateUserBankAccountAsync(userId, currency);
            return CreatedAtAction("GetUserSpecificBankAccount", new { userId, bankAccountNumber = createdBankAccount.Number }, createdBankAccount);
        }
        
        [HttpGet, Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetUserAllBankAccounts(Guid userId)
        {
            return Ok(await _adminBankAccountService.GetUserAllBankAccountsAsync(userId));
        }

        [HttpGet("{bankAccountNumber}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BankAccountDto>> GetUserSpecificBankAccount(Guid userId, string bankAccountNumber)
        {
            return Ok(await _adminBankAccountService.GetUserBankAccountAsync(userId, bankAccountNumber));
        }

        [HttpDelete("{bankAccountNumber}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CloseBankAccount(Guid userId, string bankAccountNumber)
        {
            await _adminBankAccountService.DeleteUserBankAccountAsync(userId, bankAccountNumber);
            return NoContent();
        }
    }
}
