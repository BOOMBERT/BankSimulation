﻿using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.SecurityQuestion;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Customer
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
        public async Task<ActionResult<SecurityQuestionOutDto>> GetOwnSecurityQuestion()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _securityQuestionService.GetOnlyQuestionByAccessTokenAsync(accessTokenFromHeader));
        }

        [HttpPatch("change-password"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePasswordBySecurityQuestion(ChangePasswordBySecurityQuestionDto changePasswordDto)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            await _securityQuestionService.UpdateUserPasswordBySecurityQuestionAnswerAsync(
                accessTokenFromHeader, changePasswordDto.Answer, changePasswordDto.NewPassword);
            return NoContent();
        }
    }
}
