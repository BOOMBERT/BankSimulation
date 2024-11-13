using BankSimulation.Application.BankAccounts.Exceptions;
using BankSimulation.Application.BankAccounts.Exceptions.Operations;
using BankSimulation.Application.BankAccounts.Interfaces;
using BankSimulation.Application.Common.Interfaces;
using BankSimulation.Application.Users.Exceptions;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.BankAccounts.Services
{
    public class AdminBankAccountOperationsService : IAdminBankAccountOperationsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountOperationsRepository _bankAccountOperationsRepository;
        private readonly IMoneyOperationsService _moneyOperationsService;

        public AdminBankAccountOperationsService(
            IUserRepository userRepository,
            IBankAccountRepository bankAccountRepository,
            IBankAccountOperationsRepository bankAccountOperationsRepository,
            IMoneyOperationsService moneyOperationsService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _bankAccountOperationsRepository = bankAccountOperationsRepository ?? throw new ArgumentNullException(nameof(bankAccountOperationsRepository));
            _moneyOperationsService = moneyOperationsService ?? throw new ArgumentNullException(nameof(moneyOperationsService));
        }

        public async Task DepositUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency)
        {
            if (amount <= 0)
            {
                throw new IncorrectAmountToDepositException($"{bankAccountNumber} : {amount}");
            }

            var bankAccountCurrencyInDb = await GetValidatedUserBankAccountCurrencyAsync(userId, bankAccountNumber);

            if (await _bankAccountRepository.AlreadyDeletedAsync(bankAccountNumber))
            {
                throw new BankAccountAlreadyDeletedException(bankAccountNumber);
            }

            if (currency != bankAccountCurrencyInDb)
            {
                amount = await _moneyOperationsService.ExchangeCurrencyAsync(amount, currency, bankAccountCurrencyInDb);
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
                throw new IncorrectAmountToWithdrawException($"{bankAccountNumber} : {amount}");
            }

            var bankAccountCurrencyInDb = await GetValidatedUserBankAccountCurrencyAsync(userId, bankAccountNumber);

            if (currency != bankAccountCurrencyInDb)
            {
                amount = await _moneyOperationsService.ExchangeCurrencyAsync(amount, currency, bankAccountCurrencyInDb);
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
