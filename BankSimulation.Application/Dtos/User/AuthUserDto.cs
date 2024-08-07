using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Dtos.User
{
    public class AuthUserDto
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public ICollection<AccessRole> accessRoles { get; set; }
    }
}
