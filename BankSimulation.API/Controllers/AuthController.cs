using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> RegisterUser(CreateUserDto userToCreate)
        {
            try
            {
                var user = await _userService.CreateUserAsync(userToCreate);
                return CreatedAtAction("GetUser", "Users", new { email = user.Email }, user);
            }
            catch (EmailAlreadyExistsException ex)
            {
                var errorResponse = new ErrorResponse()
                {
                    Title = $"Problem with the email address: '{ex.Email}'",
                    Detail = ex.Message
                };
                return Conflict(errorResponse);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccessTokenDto>> Login(LoginUserDto userToLogin)
        {
            try
            {
                var (accessToken, refreshToken) = await _authService.AuthenticateUserAsync(userToLogin);
                SetRefreshToken(refreshToken);
                return Ok(accessToken);

            }
            catch (UnauthorizedAccessException ex)
            {
                var errorResponse = new ErrorResponse()
                {
                    Title = $"Problem with the credentials.",
                    Detail = ex.Message
                };
                return Unauthorized(errorResponse);
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccessTokenDto>> RefreshTokens(AccessTokenDto accessTokenToRefresh)
        {
            var refreshTokenFromCookie = Request.Cookies["refreshToken"];

            try
            {
                var (accessToken, refreshToken) = await _authService.RefreshTokensAsync(accessTokenToRefresh.AccessToken, refreshTokenFromCookie);
                SetRefreshToken(refreshToken);
                return Ok(accessToken);
            }
            catch (SecurityTokenException ex)
            {
                var errorResponse = new ErrorResponse()
                {
                    Title = $"Problem with the tokens.",
                    Detail = ex.Message
                };
                return Unauthorized(errorResponse);
            }
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
