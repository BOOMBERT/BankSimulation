using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserAuthService _userAuthService;

        public UsersController(IUserService userService, IUserAuthService userAuthService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
        }

        [HttpGet, Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _userService.GetUserViaAccessTokenAsync(accessTokenFromHeader));
        }

        [HttpPatch("change-password"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _userService.UpdateUserPasswordAsync(
                accessTokenFromHeader, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword));
        }

        [HttpPatch("change-email"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<bool>> ChangeEmail(ChangeEmailDto changeEmailDto)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            return Ok(await _userService.UpdateUserEmailAsync(
                accessTokenFromHeader, changeEmailDto.CurrentEmail, changeEmailDto.NewEmail));
        }
    }
}