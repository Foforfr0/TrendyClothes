using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Auth {
    public class ManageJWTToken {

        private readonly IConfiguration _config;

        public ManageJWTToken (IConfiguration config) {
            _config = config;
        }

        public string GenerateToken (string username, string role) {
            SymmetricSecurityKey? securityKey = new SymmetricSecurityKey (
                Encoding.UTF8.GetBytes (_config["Jwt:Key"] ?? "bvfder5t6uio98765resdcvbnbgfde456yuiokjhgty65redfghuytrfdvfghp"));
            SigningCredentials? credentials = new SigningCredentials (
                securityKey, SecurityAlgorithms.HmacSha256);

            Claim[]? claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken? token = new JwtSecurityToken (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes (double.Parse (_config["Jwt:ExpiresInMinutes"] ?? "100080")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler ().WriteToken (token);
        }

    }
}
