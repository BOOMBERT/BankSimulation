using BankSimulation.Application.Auth.Dtos;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.Common.Dtos;
using BankSimulation.Application.Users.Dtos;
using BankSimulation.Application.Users.Interfaces;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Auth
{
    [Route("api/users/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IAuthService _authService;

        public AuthController(IAdminUserService adminUserService, IAuthService authService)
        {
            _adminUserService = adminUserService ?? throw new ArgumentNullException(nameof(adminUserService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("register"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(CreateUserDto userToCreate)
        {
            var createdUser = await _adminUserService.CreateUserAsync(userToCreate);
            return CreatedAtAction("GetUserById", "AdminUser", new { userId = createdUser.Id }, null);
        }

        [HttpPost("register-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAdmin()
        {
            var createdUser = await _adminUserService.CreateAdminAsync();
            return CreatedAtAction("GetUserById", "AdminUser", new { userId = createdUser.Id }, null);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccessTokenDto>> LoginUser(LoginDto userToLogin)
        {
            var (accessToken, refreshToken) = await _authService.AuthenticateUserAsync(userToLogin);
            SetRefreshToken(refreshToken);
            return Ok(accessToken);
        }

        [HttpPost("refresh"), Authorize(Roles = nameof(AccessRole.Customer) + "," + nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccessTokenDto>> RefreshTokens()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            string? refreshTokenFromCookie = Request.Cookies["refreshToken"];

            var (accessToken, refreshToken) = await _authService.RefreshUserTokensAsync(accessTokenFromHeader, refreshTokenFromCookie);
            SetRefreshToken(refreshToken);
            return Ok(accessToken);
        }

        private void SetRefreshToken(RefreshTokenDto refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.ExpirationDate
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
