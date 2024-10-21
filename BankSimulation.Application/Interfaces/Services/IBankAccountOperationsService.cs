namespace BankSimulation.Application.Interfaces.Services
{
    public interface IBankAccountOperationsService
    {
        Task TransferMoneyAsync(string accessToken, string senderBankAccountNumber, string recipientBankAccountNumber, decimal amount);
    }
}
