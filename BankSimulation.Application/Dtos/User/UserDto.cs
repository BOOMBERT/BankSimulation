namespace BankSimulation.Application.Dtos.User
{
    public record UserDto(Guid Id, string FirstName, string LastName, string Email, DateTime CreationDate, bool IsDeleted);
}
