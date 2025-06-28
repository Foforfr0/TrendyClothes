using ProductBuyerService.Services.Implements;
using ProductBuyerService.Services.Intefaces;

namespace ProductBuyerService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IConsultProductService, ConsultProductService> ();
        }
    }
}