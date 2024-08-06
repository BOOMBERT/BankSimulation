namespace BankSimulation.Application.Dtos.Auth
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
