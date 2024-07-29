using BankSimulation.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace BankSimulation.Application.Dtos.User
{
    public class UpdateUserDto
    {
        [StringLength(64, MinimumLength = 1)]
        public required string FirstName { get; set; }
        [StringLength(64, MinimumLength = 1)]
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [PasswordValidation(8, 256)]
        public required string Password { get; set; }
    }
}
