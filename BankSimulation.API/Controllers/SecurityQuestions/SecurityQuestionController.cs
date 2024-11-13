using BankSimulation.Application.Common.Dtos;
using BankSimulation.Application.SecurityQuestions.Dtos;
using BankSimulation.Application.SecurityQuestions.Interfaces;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.SecurityQuestions
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
        public async Task<ActionResult<SecurityQuestionDto>> GetOwnSecurityQuestion()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _securityQuestionService.GetOnlyQuestionByAccessTokenAsync(accessTokenFromHeader));
        }

        [HttpPatch("change-password"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePasswordBySecurityQuestion(ChangePasswordBySecurityQuestionDto dataToChangePassword)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            await _securityQuestionService.UpdateUserPasswordBySecurityQuestionAnswerAsync(
                accessTokenFromHeader, dataToChangePassword.Answer, dataToChangePassword.NewPassword);
            return NoContent();
        }
    }
}
