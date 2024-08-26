﻿using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers
{
    [Route("api/users/security-question")]
    [ApiController]
    public class SecurityQuestionController : ControllerBase
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService ?? throw new ArgumentNullException(nameof(securityQuestionService));
        }

        [HttpGet, Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetSecurityQuestion()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _securityQuestionService.GetOnlySecurityQuestionByAccessTokenAsync(accessTokenFromHeader));
        }

        [HttpPatch("change-password"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ChangePasswordBySecurityQuestion(ChangePasswordBySecurityQuestionDto changePasswordDto)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _securityQuestionService.UpdateUserPasswordBySecurityQuestionAnswerAsync
                (accessTokenFromHeader, changePasswordDto.Answer, changePasswordDto.NewPassword));
        }
    }
}
