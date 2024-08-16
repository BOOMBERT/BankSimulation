using AutoMapper;
using AutoMapper.QueryableExtensions;
using BankSimulation.Application.Dtos.Auth;
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
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
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

        public async Task<AuthUserDto?> GetUserAuthDataAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .ProjectTo<AuthUserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task AddUserRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<RefreshTokenDto?> GetRefreshTokenByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.UserId == userId)
                .Select(rt => new RefreshTokenDto(rt.Token, rt.ExpirationDate))
                .SingleOrDefaultAsync();
        }
    
        public async Task DeleteRefreshTokenByUserIdAsync(Guid userId)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<IList<AccessRole>?> GetUserAccessRolesAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.AccessRoles)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
