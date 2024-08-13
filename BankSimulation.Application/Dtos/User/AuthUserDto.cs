using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Dtos.User
{
    public record AuthUserDto(Guid Id, string Password, bool IsDeleted, ICollection<AccessRole> AccessRoles);
}
