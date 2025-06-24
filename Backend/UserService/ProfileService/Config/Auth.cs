using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProfileService.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services, WebApplicationBuilder builder) {
            builder.Services.AddAuthentication (options => {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie (CookieAuthenticationDefaults.AuthenticationScheme, options => {
                    options.LoginPath = "/User/Auth/Login";
                    options.LogoutPath = "/User/Auth/Logout";
                    options.AccessDeniedPath = "/User/Auth/AccessDenied";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromDays (7);
                })
                .AddJwtBearer (JwtBearerDefaults.AuthenticationScheme, options => {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (
                            builder.Configuration["Jwt:Key"] ?? "default-key")),

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
        }
    }
}