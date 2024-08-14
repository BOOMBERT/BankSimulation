namespace BankSimulation.Application.Dtos.User
{
    public sealed record CreateUserDto(string FirstName, string LastName, string Email, string Password);
}
