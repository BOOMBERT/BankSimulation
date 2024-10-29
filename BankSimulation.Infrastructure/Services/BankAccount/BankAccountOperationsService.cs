using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Exceptions.BankAccount.Operations;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Infrastructure.Services
{
    public class BankAccountOperationsService : IBankAccountOperationsService
    {
        private readonly IBankAccountOperationsRepository _bankAccountOperationsRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserAuthService _userAuthService;
        private readonly IMoneyOperationsService _moneyOperationsService;
        
        public BankAccountOperationsService(
            IBankAccountOperationsRepository bankAccountOperationsRepository,
            IBankAccountRepository bankAccountRepository,
            IUserAuthService userAuthService,
            IMoneyOperationsService moneyOperationsService)
        {
            _bankAccountOperationsRepository = bankAccountOperationsRepository ?? throw new ArgumentNullException(nameof(bankAccountOperationsRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
            _moneyOperationsService = moneyOperationsService;
        }

        public async Task TransferMoneyAsync(string accessToken, string senderBankAccountNumber, string recipientBankAccountNumber, decimal amount)
        {
            if (amount <= 0)
            {
                throw new IncorrectAmountToTransferException($"{senderBankAccountNumber} : {amount}");
            }

            if (senderBankAccountNumber == recipientBankAccountNumber)
            {
                throw new InvalidTransferToSameBankAccountException(senderBankAccountNumber);
            }

            var userId = _userAuthService.GetUserIdFromJwt(accessToken);

            var senderBankAccountCurrencyInDb = await GetValidatedUserBankAccountCurrencyAsync(senderBankAccountNumber, userId);
            var recipientBankAccountCurrencyInDb = await GetValidatedUserBankAccountCurrencyAsync(recipientBankAccountNumber);

            if (await _bankAccountRepository.GetBalanceAsync(senderBankAccountNumber) < amount)
            {
                throw new BankAccountBalanceTooLowException(senderBankAccountNumber);
            }

            await _bankAccountRepository.WithdrawMoneyAsync(amount, senderBankAccountNumber);
            var senderAmount = amount;

            if (senderBankAccountCurrencyInDb != recipientBankAccountCurrencyInDb)
            {
                amount = await _moneyOperationsService.ExchangeCurrencyAsync(amount, senderBankAccountCurrencyInDb, recipientBankAccountCurrencyInDb);
            }

            await _bankAccountRepository.DepositMoneyAsync(amount, recipientBankAccountNumber);

            var transfer = new Transfer
            {
                SenderAmount = senderAmount,
                RecipientAmount = amount,
                SenderBankAccountNumber = senderBankAccountNumber,
                RecipientBankAccountNumber = recipientBankAccountNumber
            };

            await _bankAccountOperationsRepository.AddTransferAsync(transfer);
            await _bankAccountOperationsRepository.SaveChangesAsync();
        }

        private async Task<Currency> GetValidatedUserBankAccountCurrencyAsync(string bankAccountNumber, Guid? userId = null)
        {
            Currency? bankAccountCurrency;

            if (userId == null)
            {
                bankAccountCurrency = await _bankAccountRepository.GetCurrencyAsync(bankAccountNumber);
            }
            else
            { 
                bankAccountCurrency = await _bankAccountRepository.GetCurrencyAsync(bankAccountNumber, (Guid)userId);
            }

            if (bankAccountCurrency == null)
            {
                throw new BankAccountDoesNotExistException(bankAccountNumber);
            }

            if (await _bankAccountRepository.AlreadyDeletedAsync(bankAccountNumber))
            {
                throw new BankAccountAlreadyDeletedException(bankAccountNumber);
            }

            return (Currency)bankAccountCurrency;
        }
    }
}
