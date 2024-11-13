using BankSimulation.Application.Common.Dtos;
using BankSimulation.Application.Users.Dtos;
using BankSimulation.Application.Users.Interfaces;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulation.API.Controllers.Users
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService ?? throw new ArgumentNullException(nameof(adminUserService));
        }

        [HttpGet("{userId}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
        {
            return Ok(await _adminUserService.GetUserByIdAsync(userId));
        }

        [HttpGet("by-email/{email}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            return Ok(await _adminUserService.GetUserByEmailAsync(email));
        }

        [HttpGet("by-bank-account-number/{bankAccountNumber}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<UserDto>> GetUserByBankAccountNumber(string bankAccountNumber)
        {
            return Ok(await _adminUserService.GetUserByBankAccountNumberAsync(bankAccountNumber));
        }

        [HttpPut("{userId}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid userId, CreateUserDto dataToUpdateUser)
        {
            await _adminUserService.UpdateUserAsync(userId, dataToUpdateUser);
            return NoContent();
        }

        [HttpPatch("{userId}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> UpdateUserPartially(Guid userId, JsonPatchDocument<CreateUserDto> patchDocument)
        {
            await _adminUserService.UpdateUserPartiallyAsync(userId, patchDocument);
            return NoContent();
        }

        [HttpDelete("{userId}"), Authorize(Roles = nameof(AccessRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _adminUserService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}
