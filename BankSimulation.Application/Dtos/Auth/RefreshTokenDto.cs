namespace BankSimulation.Application.Dtos.Auth
{
    public record RefreshTokenDto(string Token, DateTime ExpirationDate);
}
