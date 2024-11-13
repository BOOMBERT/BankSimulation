using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.BankAccounts.Exceptions;
using BankSimulation.Application.BankAccounts.Exceptions.Operations;
using BankSimulation.Application.BankAccounts.Interfaces;
using BankSimulation.Application.Common.Interfaces;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.BankAccounts.Services
{
    public class BankAccountOperationsService : IBankAccountOperationsService
    {
        private readonly IBankAccountOperationsRepository _bankAccountOperationsRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITokenService _tokenService;
        private readonly IMoneyOperationsService _moneyOperationsService;

        public BankAccountOperationsService(
            IBankAccountOperationsRepository bankAccountOperationsRepository,
            IBankAccountRepository bankAccountRepository,
            ITokenService tokenService,
            IMoneyOperationsService moneyOperationsService)
        {
            _bankAccountOperationsRepository = bankAccountOperationsRepository ?? throw new ArgumentNullException(nameof(bankAccountOperationsRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _moneyOperationsService = moneyOperationsService ?? throw new ArgumentNullException(nameof(moneyOperationsService));
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

            var userId = _tokenService.GetUserIdFromJwt(accessToken);

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
