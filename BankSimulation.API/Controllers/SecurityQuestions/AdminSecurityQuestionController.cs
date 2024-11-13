using BankSimulation.Application.Common.Dtos;
using BankSimulation.Application.SecurityQuestions.Dtos;
using BankSimulation.Application.SecurityQuestions.Interfaces;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.SecurityQuestions
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminSecurityQuestionController : ControllerBase
    {
        private readonly IAdminSecurityQuestionService _adminSecurityQuestionService;

        public AdminSecurityQuestionController(IAdminSecurityQuestionService adminSecurityQuestionService)
        {
            _adminSecurityQuestionService = adminSecurityQuestionService ?? throw new ArgumentNullException(nameof(adminSecurityQuestionService));
        }

        [HttpPost("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserSecurityQuestion(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate)
        {
            await _adminSecurityQuestionService.SetUserSecurityQuestionAsync(userId, securityQuestionToCreate);
            return CreatedAtAction(nameof(GetUserSecurityQuestion), new { userId }, null);
        }

        [HttpGet("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SecurityQuestionDto>> GetUserSecurityQuestion(Guid userId)
        {
            return Ok(await _adminSecurityQuestionService.GetSecurityQuestionByUserIdAsync(userId));
        }

        [HttpPut("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserSecurityQuestion(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate)
        {
            await _adminSecurityQuestionService.ChangeSecurityQuestionByUserIdAsync(userId, securityQuestionToCreate);
            return NoContent();
        }

        [HttpDelete("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserSecurityQuestion(Guid userId)
        {
            await _adminSecurityQuestionService.DeleteSecurityQuestionByUserIdAsync(userId);
            return NoContent();
        }
    }
}
