using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers
{
    [Route("api/users/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> RegisterUser(CreateUserDto userToCreate)
        {
            var user = await _userService.CreateUserAsync(userToCreate);
            return CreatedAtAction("GetUser", "Users", new { email = user.Email }, user);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccessTokenDto>> Login(LoginUserDto userToLogin)
        {
            var (accessToken, refreshToken) = await _authService.AuthenticateUserAsync(userToLogin);
            SetRefreshToken(refreshToken);
            return Ok(accessToken);
        }

        [HttpPost("refresh"), Authorize(Roles = nameof(AccessRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccessTokenDto>> RefreshTokens()
        {
            string accessTokenFromHeader = Request.Headers.Authorization.ToString().Split(' ')[1];
            string? refreshTokenFromCookie = Request.Cookies["refreshToken"];

            var (accessToken, refreshToken) = await _authService.RefreshTokensAsync(accessTokenFromHeader, refreshTokenFromCookie);
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
