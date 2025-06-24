using ProductService.Services.Implements;
using ProductService.Services.Intefaces;

namespace ProductService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IConsultTagsService, ConsultTagsService> ();
        }
    }
}