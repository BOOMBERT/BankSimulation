using AutoMapper;
using AutoMapper.QueryableExtensions;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users
                .AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UserDto?> GetUserDtoByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserDto?> GetUserDtoByEmailAsync(string email)
        {
            return await _context.Users
               .AsNoTracking()
               .Where(u => u.Email == email)
               .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
               .SingleOrDefaultAsync();
        }

        public async Task<string?> GetUserPasswordByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.Password)
                .SingleOrDefaultAsync();
        }

        public async Task<AuthUserDto?> GetUserAuthDataByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .ProjectTo<AuthUserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<AccessRole>?> GetUserAccessRolesAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.AccessRoles)
                .SingleOrDefaultAsync();
        }

        public async Task<Guid?> GetUserIdByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => (Guid?)u.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetUserEmailByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.Email)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> UserAlreadyDeletedByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.IsDeleted)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EmailAlreadyExistsAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }

        public async Task UpdateUserByIdAsync(Guid userId, AdminUpdateUserDto updateUserDto)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u
                .SetProperty(x => x.FirstName, updateUserDto.FirstName)
                .SetProperty(x => x.LastName, updateUserDto.LastName)
                .SetProperty(x => x.Email, updateUserDto.Email)
                .SetProperty(x => x.Password, updateUserDto.Password));
        }

        public async Task UpdateUserPasswordAsync(Guid userId, string newPassword)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.Password, newPassword));
        }

        public async Task UpdateUserEmailAsync(Guid userId, string newEmail)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.Email, newEmail));
        }

        public async Task DeleteUserByIdAsync(Guid userId)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsDeleted, true));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
