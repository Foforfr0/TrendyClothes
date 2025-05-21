using Backend.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services, WebApplicationBuilder builder) {

            /*services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (builder.Configuration["Jwt:Key"] ?? "jusdytq7yiopdndlbcav65768902eioha09876tfvghjkw")),

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
            */
            builder.Services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (builder.Configuration["Jwt:Key"] ?? "jusdytq7yiopdndlbcav65768902eioha09876tfvghjkw")),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents {
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
            services.AddAuthorization ();
            services.AddScoped<ManageJWTToken> ();
        }
    }
}