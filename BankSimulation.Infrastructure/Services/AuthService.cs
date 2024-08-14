using AutoMapper;
using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.Auth;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankSimulation.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IMapper mapper, IUserService userService, IUserRepository userRepository)
        {
             _configuration = configuration ?? throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginUserDto userToAuth)
        {
            var user = await _userService.GetUserAuthDataAsync(userToAuth.Email);

            if (user == null || !VerifyUserPassword(userToAuth.Password, user.Password))
            {
                throw new InvalidCredentialsException(userToAuth.Email);
            }

            if (user.IsDeleted) { throw new UserAlreadyDeletedException(user.Id.ToString()); }

            var storedRefreshToken = await _userRepository.GetUserRefreshTokenAsync(user.Id);

            if (storedRefreshToken != null)
            {
                _userRepository.DeleteUserRefreshToken(storedRefreshToken);
            }
            var newRefreshToken = await CreateUserRefreshTokenAsync(user.Id);

            return (
                new AccessTokenDto(GenerateAccessToken(newRefreshToken.UserId, user.AccessRoles)),
                new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpirationDate)
            );
        }

        public async Task<(AccessTokenDto, RefreshTokenDto)> RefreshTokensAsync(string accessToken, string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) { throw new InvalidTokenFormatException(refreshToken); }

            var userSubClaim = GetSpecificClaimFromJwt(accessToken, JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(userSubClaim.Value, out var userIdFromAccessToken)) { throw new InvalidTokenFormatException(accessToken); }
            
            var storedRefreshToken = await _userRepository.GetUserRefreshTokenAsync(userIdFromAccessToken);

            if (storedRefreshToken == null || storedRefreshToken.Token != refreshToken || storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException(refreshToken);
            }

            _userRepository.DeleteUserRefreshToken(storedRefreshToken);
            var newRefreshToken = await CreateUserRefreshTokenAsync(storedRefreshToken.UserId);

            var userRoles = await _userRepository.GetUserAccessRolesAsync(newRefreshToken.UserId) ?? Enumerable.Empty<AccessRole>();

            return (
                new AccessTokenDto (GenerateAccessToken(newRefreshToken.UserId, userRoles)),
                new RefreshTokenDto (newRefreshToken.Token, newRefreshToken.ExpirationDate)
                );
        }

        private IEnumerable<Claim> GetAllClaimsFromJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadToken(token) as JwtSecurityToken ?? throw new InvalidTokenFormatException(token);

            if (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return jwtSecurityToken.Claims.ToList();
            }
            else { throw new InvalidTokenFormatException(token); }
        }

        private Claim GetSpecificClaimFromJwt(string token, string claimName)
        {
            var tokenClaims = GetAllClaimsFromJwt(token);
            var claim = tokenClaims.FirstOrDefault(c => c.Type == claimName);

            if (claim == null) { throw new InvalidTokenFormatException(token); }
            return claim;
        }

        private async Task<RefreshToken> CreateUserRefreshTokenAsync(Guid userId)
        {
            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId = userId;

            await _userRepository.AddUserRefreshTokenAsync(refreshToken);
            await _userRepository.SaveChangesAsync();

            return refreshToken;
        }

        private string GenerateAccessToken(Guid userId, IEnumerable<AccessRole> userAccessRoles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            };

            foreach (var role in userAccessRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (!int.TryParse(_configuration["JwtSettings:AccessToken:ExpirationInMinutes"], out var expiresInMinutes))
            {
                throw new ArgumentException("Invalid expiration time for access token.");
            }

            var expires = TimeSpan.FromMinutes(expiresInMinutes);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(expires),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            if (!int.TryParse(_configuration["JwtSettings:RefreshToken:ExpirationInDays"], out var expiresInDays))
            {
                throw new ArgumentException("Invalid expiration time for refresh token.");
            }

            var expires = TimeSpan.FromDays(expiresInDays);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpirationDate = DateTime.UtcNow.Add(expires),
            };

            return refreshToken;
        }

        private bool VerifyUserPassword(string plainPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, passwordHash);
        }
    }
}
