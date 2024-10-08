﻿using AutoMapper;
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

        public async Task AddAsync(User user)
        {
            await _context.Users
                .AddAsync(user);
        }

        public async Task<User?> GetAsync(Guid userId)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UserDto?> GetDtoAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<UserDto?> GetDtoAsync(string email)
        {
            return await _context.Users
               .AsNoTracking()
               .Where(u => u.Email == email)
               .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
               .SingleOrDefaultAsync();
        }

        public async Task<AuthUserDto?> GetAuthDataAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .ProjectTo<AuthUserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetPasswordAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.Password)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetEmailAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.Email)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AccessRole>?> GetAccessRolesAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.AccessRoles)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid userId, AdminUpdateUserDto updateUserDto)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u
                .SetProperty(x => x.FirstName, updateUserDto.FirstName)
                .SetProperty(x => x.LastName, updateUserDto.LastName)
                .SetProperty(x => x.Email, updateUserDto.Email)
                .SetProperty(x => x.Password, updateUserDto.Password));
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPassword)
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

        public async Task DeleteAsync(Guid userId)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsDeleted, true));
        }

        public async Task<bool> AlreadyDeletedAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.IsDeleted)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> AlreadyExistsAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> AlreadyExistsAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> AlreadyExistsExceptSpecificUserAsync(string email, Guid userId)
        {
            var query = await _context.Users
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync();

            return !(query == null || query.Id == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
