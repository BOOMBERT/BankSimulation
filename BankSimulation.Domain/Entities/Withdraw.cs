﻿namespace BankSimulation.Domain.Entities
{
    public class Withdraw
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public BankAccount BankAccount { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
