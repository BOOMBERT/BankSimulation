using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankSimulation.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration, IMapper mapper)
        {
             _configuration = configuration ?? throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public string GenerateAccessToken(AuthUserDto user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (!int.TryParse(_configuration["JwtSettings:AccessToken:ExpirationInMinutes"], out var expiresInMinutes))
            {
                throw new ArgumentException("Invalid expiration time for access token.");
            }

            var expiresIn = TimeSpan.FromMinutes(expiresInMinutes);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(expiresIn),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public bool VerifyUserPassword(string plainPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, passwordHash);
        }
    }
}
