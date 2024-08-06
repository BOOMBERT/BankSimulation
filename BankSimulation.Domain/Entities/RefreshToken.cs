namespace BankSimulation.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
