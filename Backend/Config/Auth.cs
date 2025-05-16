using Backend.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Backend.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services, WebApplicationBuilder builder) {

            services.AddAuthentication (opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer (opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (builder.Configuration["Jwt:Key"])),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    opt.Events = new JwtBearerEvents {
                        OnMessageReceived = context => {
                            context.Request.Cookies.TryGetValue ("jwtToken", out string? jwtToken);
                            if (!string.IsNullOrEmpty (jwtToken))
                                context.Token = jwtToken;
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context => {
                            Console.WriteLine ("Invalid jwtToken: " + context.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization (options => {
                options.AddPolicy ("Administrator", policy =>
                    policy.RequireClaim (ClaimTypes.Role, "admin"));
                options.AddPolicy ("Seller/Buyer", policy =>
                    policy.RequireClaim (ClaimTypes.Role, "seller/buyer"));
            });
            services.AddScoped<ManageJWTToken> ();
        }
    }
}