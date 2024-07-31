using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto user)
        {
            if (await _userRepository.EmailAlreadyExistsAsync(user.Email))
            {
                throw new EmailAlreadyExistsException(user.Email);
            }

            var userEntity = _mapper.Map<User>(user);
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userRepository.AddUserAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            var userToReturn = _mapper.Map<UserDto>(userEntity);
            return userToReturn;
        }

        public async Task<UserDto?> GetUserAsync(Guid? id = null, string? email = null)
        {
            User? userEntity = null;

            if (id != null)
            {
                userEntity = await _userRepository.GetUserByIdAsync((Guid)id);
            }
            else if (email != null)
            {
                userEntity = await _userRepository.GetUserByEmailAsync(email);
            }

            if (userEntity != null)
            {
                return _mapper.Map<UserDto>(userEntity);
            }
            return null;
        }

        public async Task<AuthUserDto?> GetUserAuthDataAsync(string email)
        {
            var userEntity = await _userRepository.GetUserByEmailAsync(email);

            if (userEntity == null)
            {
                return null;
            }
            return _mapper.Map<AuthUserDto>(userEntity);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(id) ?? throw new UserNotFound(id);
            userEntity.IsDeleted = true;
            return await _userRepository.SaveChangesAsync();
        }
    }
}
