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
    public class AdminSecurityQuestionController : ControllerBase
    {
        private readonly IAdminSecurityQuestionService _adminSecurityQuestionService;

        public AdminSecurityQuestionController(IAdminSecurityQuestionService adminSecurityQuestionService)
        {
            _adminSecurityQuestionService = adminSecurityQuestionService ?? throw new ArgumentNullException(nameof(adminSecurityQuestionService));
        }

        [HttpPost("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> SetUserSecurityQuestion(Guid userId, SecurityQuestionDto securityQuestionDto)
        {
            return Ok(await _adminSecurityQuestionService.SetUserSecurityQuestionAsync(userId, securityQuestionDto));
        }

        [HttpPut("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateUserSecurityQuestion(Guid userId, SecurityQuestionDto securityQuestionDto)
        {
            return Ok(await _adminSecurityQuestionService.ChangeSecurityQuestionByUserIdAsync(userId, securityQuestionDto));
        }

        [HttpDelete("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteUserSecurityQuestion(Guid userId)
        {
            return Ok(await _adminSecurityQuestionService.DeleteSecurityQuestionByUserIdAsync(userId));
        }

        [HttpGet("{userId}/security-question"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetUserSecurityQuestion(Guid userId)
        {
            return Ok(await _adminSecurityQuestionService.GetSecurityQuestionByUserIdAsync(userId));
        }
    }
}
