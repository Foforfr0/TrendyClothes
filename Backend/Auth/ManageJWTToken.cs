using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Auth {
    public class ManageJWTToken {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly JwtSecurityTokenHandler _tokenHandler = new ();

        public ManageJWTToken (IConfiguration config, IHttpContextAccessor contextAccessor) {
            _config = config ?? throw new ArgumentNullException (nameof (config));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException (nameof (contextAccessor));
        }

        public string GenerateToken (string username, string role) {
            string? key = _config["Jwt:Key"];
            if (string.IsNullOrEmpty (key))
                throw new InvalidOperationException ("JWT key is missing from configuration.");

            SymmetricSecurityKey? securityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (key));
            SigningCredentials? credentials = new SigningCredentials (securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            double expirationMinutes = double.TryParse (_config["Jwt:ExpiresInMinutes"], out var exp) ? exp : 1440;

            JwtSecurityToken? token = new JwtSecurityToken (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes (expirationMinutes),
                signingCredentials: credentials
            );

            var jwtToken = _tokenHandler.WriteToken (token);
            SetTokenInsideCookie (jwtToken, expirationMinutes);
            return jwtToken;
        }

        private void SetTokenInsideCookie (string jwtToken, double expirationMinutes) {
            CookieOptions? cookieOptions = new CookieOptions {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes (expirationMinutes),
            };
            _contextAccessor.HttpContext?.Response.Cookies.Append ("jwtToken", jwtToken, cookieOptions);
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

                _tokenHandler.ValidateToken (jwtToken, tokenValidationParameters, out _);
                return true;
            } catch {
                return false;
            }
        }

        public string? GetUsernameFromCookie () {
            string? token = _contextAccessor.HttpContext?.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty (token) || !_tokenHandler.CanReadToken (token) || !ValidateToken (token))
                return null;

            JwtSecurityToken? jwtToken = _tokenHandler.ReadJwtToken (token);
            return jwtToken.Claims.FirstOrDefault (c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        }

        // Puedes añadir más métodos similares para obtener otros claims
        public string? GetRoleFromToken () {
            string? token = _contextAccessor.HttpContext?.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty (token) || !_tokenHandler.CanReadToken (token) || !ValidateToken (token))
                return null;

            JwtSecurityToken? jwtToken = _tokenHandler.ReadJwtToken (token);
            return jwtToken.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Role)?.Value;
        }
    }
}
