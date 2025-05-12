using Backend.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services, WebApplicationBuilder builder) {
            services.AddAuthentication (options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer (options => {
                    string? jwtKey = builder.Configuration["Jwt:Key"];
                    if (string.IsNullOrWhiteSpace (jwtKey))
                        throw new InvalidOperationException ("JWT key is missing in configuration.");

                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (jwtKey)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents {
                        OnAuthenticationFailed = context => {
                            Console.WriteLine ("Invalida JWT token: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context => {
                            string? token = context.Request.Cookies["jwt"];
                            if (!string.IsNullOrEmpty (token))
                                context.Token = token;
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization (options => {
                options.AddPolicy ("Administrator", policy =>
                    policy.RequireClaim ("role", "admin"));
            });
            services.AddAuthorization (options => {
                options.AddPolicy ("Seller/Buyer", policy =>
                    policy.RequireClaim ("role", "seller/buyer"));
            });
            services.AddScoped<ManageJWTToken> ();
        }
    }
}