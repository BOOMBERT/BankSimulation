using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.Common.Dtos;
using BankSimulation.Application.Users.Dtos;
using BankSimulation.Application.Users.Interfaces;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Customer
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dataToChangePassword)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            await _userService.UpdateUserPasswordAsync(
                accessTokenFromHeader, dataToChangePassword.CurrentPassword, dataToChangePassword.NewPassword);
            return NoContent();
        }

        [HttpPatch("change-email"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto dataToChangeEmail)
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            await _userService.UpdateUserEmailAsync(
                accessTokenFromHeader, dataToChangeEmail.CurrentEmail, dataToChangeEmail.NewEmail);
            return NoContent();
        }
    }
}