using Backend.Services.Implements.Product;
using Backend.Services.Implements.User;
using Backend.Services.Intefaces.Product;
using Backend.Services.Intefaces.User;

namespace Backend.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IAuthService, AuthService> ();
            services.AddScoped<IProfileService, ProfileService> ();
            services.AddScoped<IRegistrationService, RegistrationService> ();
            services.AddScoped<IValidateDataService, ValidateDataService> ();

            services.AddScoped<IConsultService, ConsultService> ();
        }
    }
}