﻿using BankSimulation.Application.Auth.Exceptions;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankSimulation.Application.Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtKey;
        private readonly string _accessTokenExpirationInMinutes;
        private readonly string _refreshTokenExpirationInMinutes;

        private const string JwtSecurityAlgorithm = SecurityAlgorithms.HmacSha256;
        private const string UserIdClaimName = JwtRegisteredClaimNames.Sub;

        public TokenService(string jwtKey, string accessTokenExpirationInMinutes, string refreshTokenExpirationInMinutes)
        {
            _jwtKey = jwtKey;
            _accessTokenExpirationInMinutes = accessTokenExpirationInMinutes;
            _refreshTokenExpirationInMinutes = refreshTokenExpirationInMinutes;
        }

        public IEnumerable<Claim> GetAllClaimsFromJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadToken(token) as JwtSecurityToken ?? throw new InvalidTokenFormatException(token);

            if (!jwtSecurityToken.Header.Alg.Equals(JwtSecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidTokenFormatException(token);
            }
            return jwtSecurityToken.Claims.ToArray();
        }

        public Claim GetSpecificClaimFromJwt(string token, string claimName)
        {
            var tokenClaims = GetAllClaimsFromJwt(token);
            return tokenClaims.FirstOrDefault(c => c.Type == claimName) ?? throw new InvalidTokenFormatException(token);
        }

        public Guid GetUserIdFromJwt(string token)
        {
            var subClaim = GetSpecificClaimFromJwt(token, UserIdClaimName);
            if (!Guid.TryParse(subClaim.Value, out var userId)) { throw new InvalidTokenFormatException(token); }
            return userId;
        }

        public string GenerateAccessToken(Guid userId, IEnumerable<AccessRole> userAccessRoles)
        {
            var claims = new List<Claim>
            {
                new(UserIdClaimName, userId.ToString()),
            };

            foreach (var role in userAccessRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

            var credentials = new SigningCredentials(key, JwtSecurityAlgorithm);

            if (!int.TryParse(_accessTokenExpirationInMinutes, out var expiresInMinutes))
            {
                throw new ArgumentException("Invalid expiration time for access token.");
            }

            var expires = TimeSpan.FromMinutes(expiresInMinutes);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            if (!int.TryParse(_refreshTokenExpirationInMinutes, out var expiresInMinutes))
            {
                throw new ArgumentException("Invalid expiration time for refresh token.");
            }

            var expires = TimeSpan.FromMinutes(expiresInMinutes);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpirationDate = DateTime.UtcNow.Add(expires),
            };

            return refreshToken;
        }
    }
}
