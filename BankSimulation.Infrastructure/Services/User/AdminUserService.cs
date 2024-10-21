using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Application.Validators.User;
using BankSimulation.Infrastructure.Repositories;
using BankSimulation.Infrastructure.Services.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Infrastructure.Services
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

        public async Task<UserDto> GetUserByIdAsync(Guid userId) => 
            await _userRepository.GetDtoAsync(userId) ?? throw new UserNotFoundException(userId.ToString());

        public async Task<UserDto> GetUserByEmailAsync(string email) => 
            await _userRepository.GetDtoAsync(email) ?? throw new UserNotFoundException(email);

        public async Task<UserDto> GetUserByBankAccountNumberAsync(string bankAccountNumber)
        {
            var userId = await _bankAccountRepository.GetUserIdAsync(bankAccountNumber) 
                ?? throw new BankAccountDoesNotExistException(bankAccountNumber);

            return await _userRepository.GetDtoAsync(userId) ?? throw new UserNotFoundException(userId.ToString());
        }

        public async Task UpdateUserAsync(Guid userId, AdminUpdateUserDto dataToUpdateUser)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            if (await _userRepository.AlreadyExistsExceptSpecificUserAsync(dataToUpdateUser.Email, userId))
            { 
                throw new EmailAlreadyRegisteredException(dataToUpdateUser.Email);
            }

            var updatedUser = new AdminUpdateUserDto(
                dataToUpdateUser.FirstName, dataToUpdateUser.LastName, dataToUpdateUser.Email, SecurityService.HashText(dataToUpdateUser.Password));

            await _userRepository.UpdateAsync(userId, updatedUser);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<AdminUpdateUserDto> patchDocument)
        {
            var userEntity = await _userRepository.GetAsync(userId) ?? throw new UserNotFoundException(userId.ToString());
            var userToPatch = _mapper.Map<AdminUpdateUserDto>(userEntity);

            patchDocument.ApplyTo(userToPatch);

            var validator = new AdminUpdateUserDtoValidator();
            var validationResult = validator.Validate(userToPatch);

            UtilsService.CheckValidationResult(validationResult);

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
            userEntity.Password = SecurityService.HashText(userEntity.Password);

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

            await _userRepository.DeleteAsync(userId);
            await _userRepository.SaveChangesAsync();
        }
    }
}
