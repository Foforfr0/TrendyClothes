using Backend.Services.Implements.User;
using Backend.Services.Intefaces.User;

namespace Backend.Services {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IAuthService, AuthService> ();
        }
    }
}
