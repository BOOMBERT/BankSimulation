namespace BankSimulation.Application.BankAccounts.Interfaces
{
    public interface IBankAccountOperationsService
    {
        Task TransferMoneyAsync(string accessToken, string senderBankAccountNumber, string recipientBankAccountNumber, decimal amount);
    }
}
