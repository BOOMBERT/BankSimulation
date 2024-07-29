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

        public async Task<UserDto?> GetUserAsync(Guid? id, string? email)
        {
            User? userEntity = null;

            if (email != null)
            {
                userEntity = await _userRepository.GetUserByEmailAsync(email);
            }
            else if (id != null)
            {
                userEntity = await _userRepository.GetUserByIdAsync((Guid)id);
            }

            if (userEntity != null)
            {
                return _mapper.Map<UserDto>(userEntity);
            }
            return null;
        }
    }
}
