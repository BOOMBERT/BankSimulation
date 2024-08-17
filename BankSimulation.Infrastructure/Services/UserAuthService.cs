﻿using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.Auth;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace BankSimulation.Infrastructure.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserAuthService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginUserDto userToAuth)
        {
            var user = await _userRepository.GetUserAuthDataAsync(userToAuth.Email);

            if (user == null || !VerifyUserPassword(userToAuth.Password, user.Password))
            {
                throw new InvalidCredentialsException(userToAuth.Email);
            }

            if (user.IsDeleted) { throw new UserAlreadyDeletedException(user.Id.ToString()); }

            var storedRefreshToken = await _userRepository.GetRefreshTokenByUserIdAsync(user.Id);

            if (storedRefreshToken != null)
            {
                await _userRepository.DeleteRefreshTokenByUserIdAsync(user.Id);
            }
            var newRefreshToken = await CreateUserRefreshTokenAsync(user.Id);

            return (
                new AccessTokenDto(_authService.GenerateAccessToken(newRefreshToken.UserId, user.AccessRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate)
            );
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> RefreshUserTokensAsync(string accessToken, string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) { throw new InvalidTokenFormatException(refreshToken); }

            var userIdFromAccessToken = GetUserIdFromJwt(accessToken);
            var storedRefreshToken = await _userRepository.GetRefreshTokenByUserIdAsync(userIdFromAccessToken);

            if (storedRefreshToken == null || storedRefreshToken.Token != refreshToken || storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException(refreshToken);
            }

            await _userRepository.DeleteRefreshTokenByUserIdAsync(userIdFromAccessToken);
            var newRefreshToken = await CreateUserRefreshTokenAsync(userIdFromAccessToken);

            var userRoles = await _userRepository.GetUserAccessRolesAsync(newRefreshToken.UserId) ?? Enumerable.Empty<AccessRole>();

            return (
                new AccessTokenDto(_authService.GenerateAccessToken(newRefreshToken.UserId, userRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate)
                );
        }

        public async Task<User> GetUserEntityFromJwtAsync(string token)
        {
            var userIdFromToken = GetUserIdFromJwt(token);
            var userEntity = await _userRepository.GetUserByIdAsync(userIdFromToken);
            return userEntity ?? throw new UserNotFoundException(userIdFromToken.ToString());
        }

        public bool VerifyUserPassword(string plainPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, passwordHash);
        }

        private Guid GetUserIdFromJwt(string token)
        {
            var subClaim = _authService.GetSpecificClaimFromJwt(token, JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(subClaim.Value, out var userId)) { throw new InvalidTokenFormatException(token); }
            return userId;
        }

        private async Task<RefreshToken> CreateUserRefreshTokenAsync(Guid userId)
        {
            var refreshToken = _authService.GenerateRefreshToken();
            refreshToken.UserId = userId;

            await _userRepository.AddUserRefreshTokenAsync(refreshToken);
            await _userRepository.SaveChangesAsync();

            return refreshToken;
        }
    }
}
