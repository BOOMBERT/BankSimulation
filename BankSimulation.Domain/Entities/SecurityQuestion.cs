namespace BankSimulation.Domain.Entities
{
    public class SecurityQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
