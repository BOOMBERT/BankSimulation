using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Exceptions.BankAccount.Operations;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Services.Utils;
using Microsoft.Extensions.Configuration;

namespace BankSimulation.Infrastructure.Services
{
    public class AdminBankAccountOperationsService : IAdminBankAccountOperationsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountOperationsRepository _bankAccountOperationsRepository;
        private readonly MoneyOperations _moneyOperations;

        public AdminBankAccountOperationsService(
            IUserRepository userRepository,
            IBankAccountRepository bankAccountRepository,
            IBankAccountOperationsRepository bankAccountOperationsRepository,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _bankAccountOperationsRepository = bankAccountOperationsRepository
                ?? throw new ArgumentNullException(nameof(bankAccountOperationsRepository));
            _moneyOperations = new MoneyOperations(configuration, httpClient);
        }

        public async Task DepositUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency)
        {
            if (amount <= 0)
            {
                throw new IncorrectAmountToDepositException($"{bankAccountNumber} - {amount}");
            }

            var bankAccountCurrencyInDb = await GetValidatedUserBankAccountCurrencyAsync(userId, bankAccountNumber);

            if (await _bankAccountRepository.AlreadyDeletedAsync(bankAccountNumber))
            {
                throw new BankAccountAlreadyDeletedException(bankAccountNumber);
            }

            if (currency != bankAccountCurrencyInDb)
            {
                amount = await _moneyOperations.ExchangeCurrencyAsync(amount, currency, bankAccountCurrencyInDb);
            }

            await _bankAccountRepository.DepositMoneyAsync(amount, bankAccountNumber);
            var deposit = new Deposit
            {
                Amount = amount,
                BankAccountNumber = bankAccountNumber
            };

            await _bankAccountOperationsRepository.AddDepositAsync(deposit);
            await _bankAccountOperationsRepository.SaveChangesAsync();
        }

        public async Task WithdrawUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency) 
        {
            if (amount <= 0)
            {
                throw new IncorrectAmountToWithdrawException($"{bankAccountNumber} - {amount}");
            }

            var bankAccountCurrencyInDb = await GetValidatedUserBankAccountCurrencyAsync(userId, bankAccountNumber);

            if (currency != bankAccountCurrencyInDb)
            {
                amount = await _moneyOperations.ExchangeCurrencyAsync(amount, currency, bankAccountCurrencyInDb);
            }

            if (await _bankAccountRepository.GetBalanceAsync(bankAccountNumber) < amount)
            {
                throw new BankAccountBalanceTooLowException(bankAccountNumber);
            }

            await _bankAccountRepository.WithdrawMoneyAsync(amount, bankAccountNumber);
            var withdraw = new Withdraw 
            { 
                Amount = amount, 
                BankAccountNumber = bankAccountNumber 
            };

            await _bankAccountOperationsRepository.AddWithdrawAsync(withdraw);
            await _bankAccountOperationsRepository.SaveChangesAsync();
        }

        private async Task<Currency> GetValidatedUserBankAccountCurrencyAsync(Guid userId, string bankAccountNumber)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            return await _bankAccountRepository.GetCurrencyAsync(bankAccountNumber, userId)
                ?? throw new BankAccountDoesNotExistException(bankAccountNumber);
        }
    }
}
