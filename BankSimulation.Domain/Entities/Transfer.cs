namespace BankSimulation.Domain.Entities
{
    public class Transfer
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public BankAccount SenderBankAccount { get; set; }
        public string SenderBankAccountNumber { get; set; }
        public BankAccount RecipientBankAccount { get; set; }
        public string RecipientBankAccountNumber { get; set; }
    }
}
