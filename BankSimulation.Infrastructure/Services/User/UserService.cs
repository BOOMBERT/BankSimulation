using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Application.Validators.User;
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

        public async Task<UserDto> CreateUserAsync(CreateUserDto user, AccessRole role = AccessRole.Customer)
        {
            if (await _userRepository.AlreadyExistsAsync(user.Email))
            {
                throw new EmailAlreadyRegisteredException(user.Email);
            }

            var userEntity = _mapper.Map<User>(user);
            userEntity.Password = SecurityService.HashText(user.Password);
            userEntity.AccessRoles.Add(role);

            await _userRepository.AddAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> CreateAdminAsync()
        {
            var createUser = new CreateUserDto
                (
                    FirstName: Environment.GetEnvironmentVariable("ADMIN_FIRST_NAME") ?? "",
                    LastName: Environment.GetEnvironmentVariable("ADMIN_LAST_NAME") ?? "",
                    Email: Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "",
                    Password: Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? ""
                );

            var validator = new CreateUserDtoValidator();
            var validationResult = validator.Validate(createUser);

            UtilsService.CheckValidationResult(validationResult);

            return await CreateUserAsync(createUser, AccessRole.Admin);
        }

        public async Task<UserDto> GetUserViaAccessTokenAsync(string accessToken)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            return await _userRepository.GetDtoAsync(userId) ?? throw new UserNotFoundException(userId.ToString());
        }

        public async Task UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword) { throw new IncorrectNewPasswordException(); }

            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            var userHashedPasswordFromDb = await _userRepository.GetPasswordAsync(userId) 
                ?? throw new UserNotFoundException(userId.ToString());

            if (!SecurityService.VerifyHashedText(currentPassword, userHashedPasswordFromDb))
            {
                throw new IncorrectCurrentPasswordException(userId.ToString());
            }

            await _userRepository.UpdatePasswordAsync(userId, SecurityService.HashText(newPassword));
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail)
        {
            if (currentEmail == newEmail || string.Equals(
                newEmail, Environment.GetEnvironmentVariable("ADMIN_EMAIL"), StringComparison.OrdinalIgnoreCase)) 
            { 
                throw new IncorrectNewEmailException(newEmail); 
            }

            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
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

            await _userRepository.UpdateUserEmailAsync(userId, newEmail);
            await _userRepository.SaveChangesAsync();
        }
    }
}
