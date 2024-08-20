using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Application.Validators.User;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Infrastructure.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AdminUserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(id);
            return userEntity == null ? throw new UserNotFoundException(id.ToString()) : _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var userEntity = await _userRepository.GetUserByEmailAsync(email);
            return userEntity == null ? throw new UserNotFoundException(email) : _mapper.Map<UserDto>(userEntity);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(id) ?? throw new UserNotFoundException(id.ToString());
            if (userEntity.IsDeleted) { throw new UserAlreadyDeletedException(id.ToString()); }
            userEntity.IsDeleted = true;
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserAsync(Guid userId, AdminUpdateUserDto updateUserDto)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());

            if (await _userRepository.EmailAlreadyExistsAsync(updateUserDto.Email))
            { 
                throw new EmailAlreadyRegisteredException(updateUserDto.Email);
            }
            _mapper.Map(updateUserDto, userEntity);
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userEntity.Password);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<AdminUpdateUserDto> patchDocument)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());
            var userToPatch = _mapper.Map<AdminUpdateUserDto>(userEntity);

            patchDocument.ApplyTo(userToPatch);

            var validator = new AdminUpdateUserDtoValidator();
            var validationResult = validator.Validate(userToPatch);

            ThrowValidationExceptionIfInvalid(validationResult);

            var emailOperation = patchDocument.Operations
                .SingleOrDefault(op => op.path.TrimStart('/').Equals(nameof(userToPatch.Email), StringComparison.OrdinalIgnoreCase));

            if (emailOperation != null)
            {
                if (userToPatch.Email == userEntity.Email)
                {
                    throw new IncorrectNewEmailException(userToPatch.Email);
                }

                if (await _userRepository.EmailAlreadyExistsAsync(emailOperation.value.ToString()!))
                {
                    throw new EmailAlreadyRegisteredException(emailOperation.value.ToString()!);
                }
            }

            _mapper.Map(userToPatch, userEntity);
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userEntity.Password);

            return await _userRepository.SaveChangesAsync();
        }

        private void ThrowValidationExceptionIfInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new ValidationErrorException(errors);
            }
        }
    }
}
