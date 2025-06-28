using AuthService.Services.Implements;
using AuthService.Services.Intefaces;

namespace AuthService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IAuthService, AuthService.Services.Implements.AuthService> ();
        }
    }
}