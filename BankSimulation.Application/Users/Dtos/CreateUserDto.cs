namespace BankSimulation.Application.Users.Dtos
{
    public sealed record CreateUserDto(string FirstName, string LastName, string Email, string Password);
}
