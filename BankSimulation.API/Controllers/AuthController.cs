using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Interfaces.Services;
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResponse>> Login(LoginUserDto userToLogin)
        {
            var user = await _userService.GetUserAuthDataAsync(userToLogin.Email);

           if (user == null || !_authService.VerifyUserPassword(userToLogin.Password, user.Password))
            {
                return Unauthorized();
            }

            var accessToken = new TokenResponse 
            { 
                AccessToken = _authService.GenerateAccessToken(user) 
            };
            return Ok(accessToken);
        }
    }
}
