using AutoMapper;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.Common.Utils;
using BankSimulation.Application.Users.Dtos;
using BankSimulation.Application.Users.Exceptions;
using BankSimulation.Application.Users.Interfaces;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IMapper mapper, IUserRepository userRepository, ITokenService tokenService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<UserDto> GetUserViaAccessTokenAsync(string accessToken)
        {
            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            return _mapper.Map<UserDto>(
                await _userRepository.GetAsync(userId, false) ?? throw new UserNotFoundException(userId.ToString()));
        }

        public async Task UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword) { throw new IncorrectNewPasswordException(); }

            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            var userHashedPasswordFromDb = await _userRepository.GetPasswordAsync(userId)
                ?? throw new UserNotFoundException(userId.ToString());

            if (!SecurityUtils.VerifyHashedText(currentPassword, userHashedPasswordFromDb))
            {
                throw new IncorrectCurrentPasswordException(userId.ToString());
            }

            await _userRepository.UpdatePasswordAsync(userId, SecurityUtils.HashText(newPassword));
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail)
        {
            if (currentEmail == newEmail || 
                string.Equals(newEmail, Environment.GetEnvironmentVariable("ADMIN_EMAIL"), StringComparison.OrdinalIgnoreCase))
            {
                throw new IncorrectNewEmailException(newEmail);
            }

            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            var userEmailFromDb = await _userRepository.GetEmailAsync(userId)
                ?? throw new UserNotFoundException(userId.ToString());

            if (currentEmail != userEmailFromDb)
            {
                throw new IncorrectCurrentEmailException(currentEmail);
            }

            if (await _userRepository.AlreadyExistsAsync(newEmail))
            {
                throw new EmailAlreadyRegisteredException(newEmail);
            }

            await _userRepository.UpdateEmailAsync(userId, newEmail);
            await _userRepository.SaveChangesAsync();
        }
    }
}
