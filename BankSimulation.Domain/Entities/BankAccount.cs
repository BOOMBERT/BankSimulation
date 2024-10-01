using BankSimulation.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankSimulation.Domain.Entities
{
    public class BankAccount
    {
        [Key]
        public string Number { get; set; }
        public Currency Currency { get; set; }
        public decimal Money { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        
        public User User { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
        public ICollection<Withdraw> Withdraws { get; set; } = new List<Withdraw>();
        public ICollection<Transfer> SentTransfers { get; set; } = new List<Transfer>();
        public ICollection<Transfer> ReceivedTransfers { get; set; } = new List<Transfer>();
    }
}
