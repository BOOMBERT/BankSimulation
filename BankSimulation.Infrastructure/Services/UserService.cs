using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

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
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            userEntity.AccessRoles.Add(AccessRole.Customer);

            await _userRepository.AddUserAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> GetUserViaAccessTokenAsync(string accessToken)
        {
            return _mapper.Map<UserDto>(await _userAuthService.GetUserEntityFromJwtAsync(accessToken));
        }

        public async Task<bool> UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword) { throw new IncorrectNewPasswordException(); }

            var userEntity = await _userAuthService.GetUserEntityFromJwtAsync(accessToken);

            if (!_userAuthService.VerifyUserPassword(currentPassword, userEntity.Password))
            {
                throw new IncorrectCurrentPasswordException(userEntity.Id.ToString());
            }

            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _userRepository.UpdateUser(userEntity);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail)
        {
            if (currentEmail == newEmail) 
            {
                throw new IncorrectNewEmailException(newEmail); 
            }

            if (await _userRepository.EmailAlreadyExistsAsync(newEmail))
            {
                throw new EmailAlreadyRegisteredException(newEmail);
            }

            var userEntity = await _userAuthService.GetUserEntityFromJwtAsync(accessToken);

            if (currentEmail != userEntity.Email)
            {
                throw new IncorrectCurrentEmailException(currentEmail);
            }

            userEntity.Email = newEmail;
            _userRepository.UpdateUser(userEntity);

            return await _userRepository.SaveChangesAsync();
        }
    }
}
