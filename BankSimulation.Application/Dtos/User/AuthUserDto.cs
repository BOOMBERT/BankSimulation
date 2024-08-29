using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Dtos.User
{
    public sealed record AuthUserDto(Guid Id, string Password, bool IsDeleted, ICollection<AccessRole> AccessRoles);
}
