namespace BankSimulation.Application.Users.Dtos
{
    public sealed record UserDto(Guid Id, string FirstName, string LastName, string Email, DateTime CreationDate, bool IsDeleted);
}
