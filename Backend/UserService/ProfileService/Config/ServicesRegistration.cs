using ProfileService.Services.Implements;
using ProfileService.Services.Intefaces;

namespace ProfileService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IConsultProfileService, ConsultProfileService> ();
        }
    }
}