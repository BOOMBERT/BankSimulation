namespace BankSimulation.Domain.Entities
{
    public class Deposit
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public BankAccount BankAccount { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
