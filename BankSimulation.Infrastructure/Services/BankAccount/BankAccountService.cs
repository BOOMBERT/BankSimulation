using AutoMapper;
using BankSimulation.Application.Dtos;
using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Infrastructure.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserAuthService _userAuthService;

        public BankAccountService(IMapper mapper, IBankAccountRepository bankAccountRepository, IUserAuthService userAuthService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
        }
        
        public async Task<IEnumerable<BankAccountDto>> GetAllOwnBankAccountsAsync(string accessToken)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            var userBankAccounts = await _bankAccountRepository.GetAsync(userId) 
                ?? Enumerable.Empty<BankAccount>();

            return _mapper.Map<IEnumerable<BankAccountDto>>(userBankAccounts);
        }

        public async Task<BankAccountDto> GetOwnBankAccountAsync(string accessToken, string bankAccountNumber)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            return _mapper.Map<BankAccountDto>(await _bankAccountRepository.GetAsync(userId, bankAccountNumber) 
                ?? throw new UserBankAccountDoesNotExistException(userId.ToString()));
        }
    }
}
