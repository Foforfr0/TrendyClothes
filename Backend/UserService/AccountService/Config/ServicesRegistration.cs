using AccountService.Services.Implements;
using AccountService.Services.Intefaces;

namespace AccountService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IDeleteAccountService, DeleteAccountService> ();
            services.AddScoped<IRegistrationAccountService, RegistrationAccountService> ();
            services.AddScoped<IValidateDataService, ValidateDataService> ();
        }
    }
}