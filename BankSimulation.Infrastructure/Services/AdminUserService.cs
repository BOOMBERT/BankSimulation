using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Application.Validators.User;
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

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);
            return userEntity == null ? throw new UserNotFoundException(userId.ToString()) : _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var userEntity = await _userRepository.GetUserByEmailAsync(email);
            return userEntity == null ? throw new UserNotFoundException(email) : _mapper.Map<UserDto>(userEntity);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            if (await _userRepository.UserAlreadyDeletedByIdAsync(userId))
            {
                throw new UserAlreadyDeletedException(userId.ToString());
            }

            await _userRepository.DeleteUserByIdAsync(userId);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserAsync(Guid userId, AdminUpdateUserDto updateUserDto)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());

            if (userEntity.Email == updateUserDto.Email)
            {
                throw new IncorrectNewEmailException(updateUserDto.Email);
            }

            if (await _userRepository.EmailAlreadyExistsAsync(updateUserDto.Email))
            { 
                throw new EmailAlreadyRegisteredException(updateUserDto.Email);
            }
            _mapper.Map(updateUserDto, userEntity);
            userEntity.Password = SecurityService.HashText(userEntity.Password);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<AdminUpdateUserDto> patchDocument)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId) ?? throw new UserNotFoundException(userId.ToString());
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

                if (await _userRepository.EmailAlreadyExistsAsync(emailOperation.value.ToString()!))
                {
                    throw new EmailAlreadyRegisteredException(emailOperation.value.ToString()!);
                }
            }
            _mapper.Map(userToPatch, userEntity);
            userEntity.Password = SecurityService.HashText(userEntity.Password);

            return await _userRepository.SaveChangesAsync();
        }
    }
}
