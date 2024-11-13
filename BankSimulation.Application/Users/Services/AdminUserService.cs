using AutoMapper;
using BankSimulation.Application.BankAccounts.Exceptions;
using BankSimulation.Application.Common.Utils;
using BankSimulation.Application.Users.Dtos;
using BankSimulation.Application.Users.Exceptions;
using BankSimulation.Application.Users.Interfaces;
using BankSimulation.Application.Users.Validators;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Domain.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Application.Users.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public AdminUserService(IMapper mapper, IUserRepository userRepository, IBankAccountRepository bankAccountRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto user, AccessRole role = AccessRole.Customer)
        {
            if (await _userRepository.AlreadyExistsAsync(user.Email))
            {
                throw new EmailAlreadyRegisteredException(user.Email);
            }

            var userEntity = _mapper.Map<User>(user);
            userEntity.Password = SecurityUtils.HashText(user.Password);
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

            ValidationUtils.CheckValidationResult(validationResult);

            return await CreateUserAsync(createUser, AccessRole.Admin);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId) =>
            _mapper.Map<UserDto>(
                await _userRepository.GetAsync(userId, false) ?? throw new UserNotFoundException(userId.ToString()));

        public async Task<UserDto> GetUserByEmailAsync(string email) =>
            _mapper.Map<UserDto>(
                await _userRepository.GetAsync(email, false) ?? throw new UserNotFoundException(email));

        public async Task<UserDto> GetUserByBankAccountNumberAsync(string bankAccountNumber)
        {
            var userId = await _bankAccountRepository.GetUserIdAsync(bankAccountNumber)
                ?? throw new BankAccountDoesNotExistException(bankAccountNumber);

            return _mapper.Map<UserDto>(
                await _userRepository.GetAsync(userId, false) ?? throw new UserNotFoundException(userId.ToString()));
        }

        public async Task UpdateUserAsync(Guid userId, CreateUserDto dataToUpdateUser)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            if (await _userRepository.AlreadyExistsExceptSpecificUserAsync(dataToUpdateUser.Email, userId))
            {
                throw new EmailAlreadyRegisteredException(dataToUpdateUser.Email);
            }

            await _userRepository.UpdateAsync(
                userId, 
                dataToUpdateUser.FirstName, 
                dataToUpdateUser.LastName, 
                dataToUpdateUser.Email, 
                SecurityUtils.HashText(dataToUpdateUser.Password)
            );
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<CreateUserDto> patchDocument)
        {
            var userEntity = await _userRepository.GetAsync(userId, true) ?? throw new UserNotFoundException(userId.ToString());
            var userToPatch = _mapper.Map<CreateUserDto>(userEntity);

            patchDocument.ApplyTo(userToPatch);

            var validator = new CreateUserDtoValidator();
            var validationResult = validator.Validate(userToPatch);

            ValidationUtils.CheckValidationResult(validationResult);

            var emailOperation = patchDocument.Operations
                .SingleOrDefault(op => op.path.TrimStart('/').Equals(nameof(userToPatch.Email), StringComparison.OrdinalIgnoreCase));

            if (emailOperation != null)
            {
                if (userToPatch.Email == userEntity.Email)
                {
                    throw new IncorrectNewEmailException(userToPatch.Email);
                }

                if (await _userRepository.AlreadyExistsAsync(emailOperation.value.ToString()!))
                {
                    throw new EmailAlreadyRegisteredException(emailOperation.value.ToString()!);
                }
            }

            _mapper.Map(userToPatch, userEntity);
            userEntity.Password = SecurityUtils.HashText(userEntity.Password);

            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            if (await _userRepository.AlreadyDeletedAsync(userId))
            {
                throw new UserAlreadyDeletedException(userId.ToString());
            }

            await _bankAccountRepository.DeleteAsync(userId);

            await _userRepository.SoftDeleteAsync(userId);
            await _userRepository.SaveChangesAsync();
        }
    }
}
