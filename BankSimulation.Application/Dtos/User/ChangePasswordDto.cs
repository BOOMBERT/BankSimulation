namespace BankSimulation.Application.Dtos.User
{
    public sealed record ChangePasswordDto(string CurrentPassword, string NewPassword);
}
