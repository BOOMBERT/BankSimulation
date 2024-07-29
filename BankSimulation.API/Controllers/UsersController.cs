using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(EmailErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> RegisterUser(CreateUserDto user)
        {
            try
            {
                var userEntity = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { email = userEntity.Email }, userEntity);
            }
            catch (EmailAlreadyExistsException ex)
            {
                var errorResponse = new EmailErrorResponse()
                {
                    Message = ex.Message,
                    Email = user.Email
                };
                return Conflict(errorResponse);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetUser(Guid? id, string? email)
        {
            if (id == null && email == null)
            {
                return BadRequest();
            }

            var user = await _userService.GetUserAsync(id, email);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}