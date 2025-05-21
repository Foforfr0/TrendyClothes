using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Auth {
    public class ManageJWTToken : ControllerBase {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;

        public ManageJWTToken (IConfiguration config, IHttpContextAccessor contextAccessor) {
            _config = config ?? throw new ArgumentNullException (nameof (config));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException (nameof (contextAccessor));
        }

        public string GenerateToken (string username, string role) {
            SymmetricSecurityKey? key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config["Jwt:Key"] ?? "jusdytq7yiopdndlbcav65768902eioha09876tfvghjkw"));
            SigningCredentials? creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);

            Claim[]? claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString())
            };

            JwtSecurityToken? token = new JwtSecurityToken (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays (7),
                signingCredentials: creds
            );

            string? jwt = new JwtSecurityTokenHandler ().WriteToken (token);

            return jwt;
        }

        public bool ValidateToken (string jwtToken) {
            try {
                string? key = _config["Jwt:Key"];
                if (string.IsNullOrEmpty (key))
                    return false;

                TokenValidationParameters? tokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (key)),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                new JwtSecurityTokenHandler ().ValidateToken (jwtToken, tokenValidationParameters, out _);
                return true;
            } catch {
                return false;
            }
        }
    }
}
