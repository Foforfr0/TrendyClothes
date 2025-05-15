using Backend.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Backend.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services, WebApplicationBuilder builder) {
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (builder.Configuration["Jwt:Key"])),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    // Permitir JWT desde cookies
                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = context => {
                            var token = context.Request.Cookies["jwt"];
                            if (!string.IsNullOrEmpty (token))
                                context.Token = token;

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context => {
                            Console.WriteLine ("Invalid JWT: " + context.Exception.Message);
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