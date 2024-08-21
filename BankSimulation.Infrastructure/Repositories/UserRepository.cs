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
        private readonly UsersContext _context;
        private readonly IMapper _mapper;

        public UserRepository(UsersContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users
                .AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailAlreadyExistsAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
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

        public async Task<bool> UserAlreadyDeletedByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.IsDeleted)
                .SingleOrDefaultAsync();
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
