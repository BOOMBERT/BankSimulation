using BankSimulation.Application.Auth.Dtos;
using BankSimulation.Application.Auth.Exceptions;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.Common.Utils;
using BankSimulation.Application.Users.Exceptions;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginDto userToAuth)
        {
            var userEntity = await _userRepository.GetAsync(userToAuth.Email, false);

            if (userEntity == null || !SecurityUtils.VerifyHashedText(userToAuth.Password, userEntity.Password))
            {
                throw new InvalidCredentialsException(userToAuth.Email);
            }

            if (userEntity.IsDeleted) { throw new UserAlreadyDeletedException(userEntity.Id.ToString()); }

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            if (await _refreshTokenRepository.AlreadyExistsAsync(userEntity.Id))
            {
                await _refreshTokenRepository.UpdateAsync(userEntity.Id, newRefreshToken.Token, newRefreshToken.ExpirationDate);
            }
            else
            {
                newRefreshToken.UserId = userEntity.Id;
                await _refreshTokenRepository.AddAsync(newRefreshToken);
            }
            await _userRepository.SaveChangesAsync();

            return (
                new AccessTokenDto(_tokenService.GenerateAccessToken(userEntity.Id, userEntity.AccessRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate));
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> RefreshUserTokensAsync(string accessToken, string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) { throw new InvalidTokenFormatException(refreshToken); }

            var userIdFromAccessToken = _tokenService.GetUserIdFromJwt(accessToken);

            if (await _userRepository.AlreadyDeletedAsync(userIdFromAccessToken)) 
            {
                throw new UserAlreadyDeletedException(userIdFromAccessToken.ToString());
            }

            var storedRefreshToken = await _refreshTokenRepository.GetAsync(userIdFromAccessToken, false);

            if (storedRefreshToken == null || storedRefreshToken.Token != refreshToken || storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException($"{userIdFromAccessToken} : {refreshToken}");
            }
            
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _refreshTokenRepository.UpdateAsync(userIdFromAccessToken, newRefreshToken.Token, newRefreshToken.ExpirationDate);
            await _userRepository.SaveChangesAsync();

            var userRoles = await _userRepository.GetAccessRolesAsync(userIdFromAccessToken);

            return (
                new AccessTokenDto(_tokenService.GenerateAccessToken(userIdFromAccessToken, userRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate));
        }
    }
}
