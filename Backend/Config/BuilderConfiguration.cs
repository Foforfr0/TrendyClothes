using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Config {
    public static class BuilderConfiguration {
        public static void ConfigureBuilder (this IServiceCollection services, WebApplicationBuilder builder) {
            services.AddOpenApi ();             // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddControllers ();
            services.AddControllers ()
                .AddJsonOptions (options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });
            services.AddCors (options => {
                options.AddPolicy ("FromFrontend", policy => {
                    policy.WithOrigins ("https://localhost:8081")    // indica qué dominios pueden hacer peticiones.
                          .AllowAnyHeader ()                                // permite cualquier encabezado (como JSON, tokens, etc).
                          .AllowAnyMethod ();                               // permite cualquier método HTTP.
                });
            });
            services.AddDbContext<TrendyClothesDBContext> (options =>
                options.UseSqlServer (
                    builder.Configuration.GetConnectionString ("SQLServer")),
                ServiceLifetime.Scoped);
            services.AddAuthentication ("Bearer")
                .AddJwtBearer ("Bearer", options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        //TODO
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey (
                            Encoding.UTF8.GetBytes (
                                builder.Configuration["Jwt:Key"] ?? "bvfder5t6uio98765resdcvbnbgfde456yuiokjhgty65redfghuytrfdvfghp"))
                    };
                });

        }
    }
}
