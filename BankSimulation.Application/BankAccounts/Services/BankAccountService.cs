using AutoMapper;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.BankAccounts.Dtos;
using BankSimulation.Application.BankAccounts.Exceptions;
using BankSimulation.Application.BankAccounts.Interfaces;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITokenService _tokenService;

        public BankAccountService(IMapper mapper, IBankAccountRepository bankAccountRepository, ITokenService tokenService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<IEnumerable<BankAccountDto>> GetAllOwnBankAccountsAsync(string accessToken)
        {
            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            var userBankAccounts = await _bankAccountRepository.GetAsync(userId, false);

            return _mapper.Map<IEnumerable<BankAccountDto>>(userBankAccounts);
        }

        public async Task<BankAccountDto> GetOwnBankAccountAsync(string accessToken, string bankAccountNumber)
        {
            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            return _mapper.Map<BankAccountDto>(
                await _bankAccountRepository.GetAsync(userId, bankAccountNumber, false) 
                    ?? throw new BankAccountDoesNotExistException($"{userId} : {bankAccountNumber}"));
        }
    }
}
