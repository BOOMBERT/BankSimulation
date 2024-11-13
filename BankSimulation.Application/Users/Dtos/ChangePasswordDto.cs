namespace BankSimulation.Application.Users.Dtos
{
    public sealed record ChangePasswordDto(string CurrentPassword, string NewPassword);
}
