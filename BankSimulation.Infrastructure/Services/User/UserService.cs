using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Services.Utils;

namespace BankSimulation.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserAuthService _userAuthService;

        public UserService(IMapper mapper, IUserRepository userRepository, IUserAuthService userAuthService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto user)
        {
            if (await _userRepository.EmailAlreadyExistsAsync(user.Email))
            {
                throw new EmailAlreadyRegisteredException(user.Email);
            }

            var userEntity = _mapper.Map<User>(user);
            userEntity.Password = SecurityService.HashText(user.Password);
            userEntity.AccessRoles.Add(AccessRole.Customer);

            await _userRepository.AddUserAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> GetUserViaAccessTokenAsync(string accessToken)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            return await _userRepository.GetUserDtoByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());
        }

        public async Task<bool> UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword) { throw new IncorrectNewPasswordException(); }

            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            var userHashedPasswordFromDb = await _userRepository.GetUserPasswordByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());

            if (!SecurityService.VerifyHashedText(currentPassword, userHashedPasswordFromDb))
            {
                throw new IncorrectCurrentPasswordException(userId.ToString());
            }
            if (SecurityService.VerifyHashedText(newPassword, userHashedPasswordFromDb))
            {
                throw new IncorrectNewPasswordException(userId.ToString());
            }

            await _userRepository.UpdatePasswordAsync(userId, SecurityService.HashText(newPassword));
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail)
        {
            if (currentEmail == newEmail) { throw new IncorrectNewEmailException(newEmail); }

            if (await _userRepository.EmailAlreadyExistsAsync(newEmail))
            {
                throw new EmailAlreadyRegisteredException(newEmail);
            }

            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            var userEmailFromDb = await _userRepository.GetUserEmailByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());

            if (currentEmail != userEmailFromDb) { throw new IncorrectCurrentEmailException(currentEmail); }

            await _userRepository.UpdateUserEmailAsync(userId, newEmail);
            return await _userRepository.SaveChangesAsync();
        }
    }
}
