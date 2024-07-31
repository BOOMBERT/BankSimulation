using System.ComponentModel.DataAnnotations;

namespace BankSimulation.Application.Dtos.User
{
    public class LoginUserDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
