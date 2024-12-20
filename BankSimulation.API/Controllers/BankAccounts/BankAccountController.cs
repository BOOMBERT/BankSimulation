﻿using BankSimulation.Application.BankAccounts.Dtos;
using BankSimulation.Application.BankAccounts.Interfaces;
using BankSimulation.Application.Common.Dtos;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Customer
{
    [Route("api/users/bank-accounts")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService ?? throw new ArgumentNullException(nameof(bankAccountService));
        }

        [HttpGet, Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetAllOwnBankAccounts()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _bankAccountService.GetAllOwnBankAccountsAsync(accessTokenFromHeader));
        }

        [HttpGet("{bankAccountNumber}"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BankAccountDto>> GetOwnSpecificBankAccount(string bankAccountNumber)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _bankAccountService.GetOwnBankAccountAsync(accessTokenFromHeader, bankAccountNumber));
        }
    }
}
