namespace BankSimulation.Application.Dtos.User
{
    public sealed record UpdateUserDto(string FirstName, string LastName, string Email, string Password);
}
