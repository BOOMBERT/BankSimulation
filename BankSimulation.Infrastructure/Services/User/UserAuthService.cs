using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.Auth;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Services.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace BankSimulation.Infrastructure.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserAuthService(IUserRepository userRepository, IAuthService authService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginUserDto userToAuth)
        {
            var userAuthData = await _userRepository.GetAuthDataAsync(userToAuth.Email);

            if (userAuthData == null || !SecurityService.VerifyHashedText(userToAuth.Password, userAuthData.Password))
            {
                throw new InvalidCredentialsException(userToAuth.Email);
            }

            if (userAuthData.IsDeleted) { throw new UserAlreadyDeletedException(userAuthData.Id.ToString()); }

            if (await _refreshTokenRepository.AlreadyExistsAsync(userAuthData.Id))
            {
                await _refreshTokenRepository.DeleteAsync(userAuthData.Id);
            }
            var newRefreshToken = await CreateUserRefreshTokenAsync(userAuthData.Id);

            return (
                new AccessTokenDto(_authService.GenerateAccessToken(newRefreshToken.UserId, userAuthData.AccessRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate));
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> RefreshUserTokensAsync(string accessToken, string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) { throw new InvalidTokenFormatException(refreshToken); }

            var userIdFromAccessToken = GetUserIdFromJwt(accessToken);
            var storedRefreshToken = await _refreshTokenRepository.GetAsync(userIdFromAccessToken);

            if (storedRefreshToken == null || storedRefreshToken.Token != refreshToken || storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException(refreshToken);
            }

            await _refreshTokenRepository.DeleteAsync(userIdFromAccessToken);
            var newRefreshToken = await CreateUserRefreshTokenAsync(userIdFromAccessToken);

            var userRoles = await _userRepository.GetAccessRolesAsync(newRefreshToken.UserId) ?? Enumerable.Empty<AccessRole>();

            return (
                new AccessTokenDto(_authService.GenerateAccessToken(newRefreshToken.UserId, userRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate));
        }

        public Guid GetUserIdFromJwt(string token)
        {
            var subClaim = _authService.GetSpecificClaimFromJwt(token, JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(subClaim.Value, out var userId)) { throw new InvalidTokenFormatException(token); }
            return userId;
        }

        private async Task<RefreshToken> CreateUserRefreshTokenAsync(Guid userId)
        {
            var refreshToken = _authService.GenerateRefreshToken();
            refreshToken.UserId = userId;

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _userRepository.SaveChangesAsync();

            return refreshToken;
        }
    }
}
