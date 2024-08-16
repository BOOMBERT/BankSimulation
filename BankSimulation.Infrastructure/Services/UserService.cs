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
    }
}
