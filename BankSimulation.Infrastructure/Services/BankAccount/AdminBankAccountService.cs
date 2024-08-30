using AutoMapper;
using BankSimulation.Application.Dtos;
using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace BankSimulation.Infrastructure.Services
{
    public class AdminBankAccountService : IAdminBankAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private static readonly Random _random = new();

        public AdminBankAccountService(IMapper mapper, IUserRepository userRepository, IBankAccountRepository bankAccountRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public async Task<bool> CreateUserBankAccountAsync(Guid userId, Currency bankAccountCurrency)
        {
            if (!await _userRepository.UserAlreadyExistsByIdAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            var bankAccount = new BankAccount 
            { 
                Number = await GenerateBankAccountNumberAsync(),
                Currency = bankAccountCurrency,
                UserId = userId, 
            };

            await _bankAccountRepository.AddBankAccountAsync(bankAccount);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserBankAccountByNumberAsync(string bankAccountNumber)
        {
            if (!await _bankAccountRepository.BankAccountNumberAlreadyExistsAsync(bankAccountNumber))
            {
                throw new UserBankAccountDoesNotExistException(bankAccountNumber);
            }
            if (await _bankAccountRepository.BankAccountAlreadyDeletedAsync(bankAccountNumber))
            {
                throw new UserBankAccountAlreadyDeletedException(bankAccountNumber);
            }
            await _bankAccountRepository.DeleteBankAccountAsync(bankAccountNumber);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<BankAccountDto> GetUserBankAccountByNumberAsync(string bankAccountNumber)
        {
            return _mapper.Map<BankAccountDto>(await _bankAccountRepository.GetBankAccountAsync(bankAccountNumber)
                ?? throw new UserBankAccountDoesNotExistException(bankAccountNumber));
        }

        public async Task<IEnumerable<BankAccountDto>> GetAllBankAccountsByUserIdAsync(Guid userId)
        {
            if (!await _userRepository.UserAlreadyExistsByIdAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }
            var userBankAccounts = await _bankAccountRepository.GetUserAllBankAccountsAsync(userId);
            if (userBankAccounts.IsNullOrEmpty())
            {
                throw new UserBankAccountDoesNotExistException(userId.ToString());
            }
            return _mapper.Map<IEnumerable<BankAccountDto>>(userBankAccounts);
        }

        private async Task<string> GenerateBankAccountNumberAsync()
        {
            string generatedBankAccountNumber;
            do 
            {
                generatedBankAccountNumber = _random.Next(100_000_000, 999_999_999).ToString();
            } while (await _bankAccountRepository.BankAccountNumberAlreadyExistsAsync(generatedBankAccountNumber));

            return generatedBankAccountNumber;
        }
    }
}
