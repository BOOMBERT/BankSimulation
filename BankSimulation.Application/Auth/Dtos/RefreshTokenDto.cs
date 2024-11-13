namespace BankSimulation.Application.Auth.Dtos
{
    public sealed record RefreshTokenDto(string Token, DateTime ExpirationDate);
}
