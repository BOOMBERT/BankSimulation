﻿namespace BankSimulation.Domain.Entities
{
    public class Transfer
    {
        public int Id { get; set; }
        public decimal SenderAmount { get; set; }
        public decimal RecipientAmount { get; set; }
        public DateTime Date { get; set; }

        public BankAccount SenderBankAccount { get; set; }
        public string SenderBankAccountNumber { get; set; }

        public BankAccount RecipientBankAccount { get; set; }
        public string RecipientBankAccountNumber { get; set; }
    }
}
