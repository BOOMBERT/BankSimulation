﻿using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Application.Validators.User;
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

            var updateUserDto = new UpdateUserDto(userEntity.Email, newPassword);
            var validator = new UpdateUserDtoValidator();
            var validationResult = validator.Validate(updateUserDto);

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

            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _userRepository.UpdateUser(userEntity);

            return await _userRepository.SaveChangesAsync();
        }
    }
}
