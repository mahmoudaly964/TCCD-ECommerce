using Application.Services_Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly double _accessTokenExpirationMinutes;

        public JwtService(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Key"]
                ?? throw new ArgumentNullException("Jwt:Key is missing from configuration");
            _jwtIssuer = configuration["Jwt:Issuer"]
                ?? throw new ArgumentNullException("Jwt:Issuer is missing from configuration");
            _jwtAudience = configuration["Jwt:Audience"]
                ?? throw new ArgumentNullException("Jwt:Audience is missing from configuration");
            _accessTokenExpirationMinutes = double.TryParse(configuration["Jwt:ExpiresInMinutes"], out var minutes)
                ? minutes
                : 30;
        }

        public string GenerateAccessToken(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Invalid user data");

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("createdAt", DateTime.UtcNow.ToString("o"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtIssuer,
                    ValidAudience = _jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret))
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
