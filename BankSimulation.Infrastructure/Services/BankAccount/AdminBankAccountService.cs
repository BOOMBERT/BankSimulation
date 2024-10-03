using AutoMapper;
using BankSimulation.Application.Dtos.BankAccount;
using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

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

        public async Task<BankAccountDto> CreateUserBankAccountAsync(Guid userId, Currency bankAccountCurrency)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            var bankAccount = new BankAccount 
            { 
                Number = await GenerateBankAccountNumberAsync(),
                Currency = bankAccountCurrency,
                UserId = userId, 
            };

            await _bankAccountRepository.AddAsync(bankAccount);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<BankAccountDto>(bankAccount);
        }

        public async Task<BankAccountDto> GetUserBankAccountAsync(Guid userId, string bankAccountNumber)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            return _mapper.Map<BankAccountDto>(await _bankAccountRepository.GetAsync(userId, bankAccountNumber)
                ?? throw new BankAccountDoesNotExistException(bankAccountNumber));
        }

        public async Task<IEnumerable<BankAccountDto>> GetUserAllBankAccountsAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            var userBankAccounts = await _bankAccountRepository.GetAsync(userId) 
                ?? Enumerable.Empty<BankAccount>();

            return _mapper.Map<IEnumerable<BankAccountDto>>(userBankAccounts);
        }

        public async Task DeleteUserBankAccountAsync(Guid userId, string bankAccountNumber)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            var bankAccountToDelete = await _bankAccountRepository.GetAsync(userId, bankAccountNumber, true)
                            ?? throw new BankAccountDoesNotExistException($"{userId} : {bankAccountNumber}");
            
            if (await _bankAccountRepository.AlreadyDeletedAsync(bankAccountNumber))
            {
                throw new BankAccountAlreadyDeletedException(bankAccountNumber);
            }

            bankAccountToDelete.IsDeleted = true;
            await _userRepository.SaveChangesAsync();
        }

        private async Task<string> GenerateBankAccountNumberAsync()
        {
            string generatedBankAccountNumber;
            do 
            {
                generatedBankAccountNumber = _random.Next(100_000, 999_999).ToString();
            } while (await _bankAccountRepository.AlreadyExistsAsync(generatedBankAccountNumber));

            return generatedBankAccountNumber;
        }
    }
}
