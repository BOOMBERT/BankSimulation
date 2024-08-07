using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
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

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet, Authorize(Roles = nameof(AccessRole.Admin))]
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

        [HttpDelete, Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                await _userService.DeleteUserAsync((Guid)id);
                return NoContent();
            }
            catch (UserNotFound ex)
            {
                var errorResponse = new ErrorResponse()
                {
                    Title = $"Problem with the user: '{ex.UserId}'",
                    Detail = ex.Message
                };
                return NotFound(errorResponse);
            }
        }
    }
}