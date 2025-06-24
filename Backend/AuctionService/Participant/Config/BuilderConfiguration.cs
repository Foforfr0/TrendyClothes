using Microsoft.EntityFrameworkCore;
using AuctionParticipantService.Entities;

namespace AuctionParticipantService.Config {
    public static class BuilderConfiguration {
        public static void ConfigureBuilder (this IServiceCollection services, WebApplicationBuilder builder) {
            builder.Services.AddOpenApi ();             // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddControllers ()
                .AddJsonOptions (options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });
            builder.Services.AddCors (options => {
                options.AddPolicy ("FromFrontend", policy => {
                    policy.WithOrigins ("http://localhost:8081")
                        .AllowCredentials ()                              // Necesary for Cookies.
                        .AllowAnyHeader ()                                // Allow any header (JSON, tokens, etc).
                        .AllowAnyMethod ();                               // Allow any HTTP method.
                });
            });
            builder.Services.AddDbContext<TrendyClothesDBContext> (options =>
                options.UseSqlServer (
                    builder.Configuration.GetConnectionString ("SQLServer")),
                    ServiceLifetime.Scoped);
        }
    }
}
