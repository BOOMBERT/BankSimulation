using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers
{
    [Route("api/admin/security-question")]
    [ApiController]
    public class AdminSecurityQuestionController : ControllerBase
    {
        private readonly IAdminSecurityQuestionService _adminSecurityQuestionService;

        public AdminSecurityQuestionController(IAdminSecurityQuestionService adminSecurityQuestionService)
        {
            _adminSecurityQuestionService = adminSecurityQuestionService ?? throw new ArgumentNullException(nameof(adminSecurityQuestionService));
        }

        [HttpPost("{email}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> SetUserSecurityQuestion(string email, SecurityQuestionDto securityQuestionDto)
        {
            return Ok(await _adminSecurityQuestionService.SetUserSecurityQuestionAsync(email, securityQuestionDto));
        }

        [HttpPut("{email}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateUserSecurityQuestion(string email, SecurityQuestionDto securityQuestionDto)
        {
            return Ok(await _adminSecurityQuestionService.ChangeSecurityQuestionByUserEmailAsync(email, securityQuestionDto));
        }

        [HttpDelete("{email}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteUserSecurityQuestion(string email)
        {
            return Ok(await _adminSecurityQuestionService.DeleteSecurityQuestionByUserEmailAsync(email));
        }

        [HttpGet("{email}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetUserSecurityQuestion(string email)
        {
            return Ok(await _adminSecurityQuestionService.GetSecurityQuestionByUserEmailAsync(email));
        }
    }
}
