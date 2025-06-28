using ProductBuyerService.DAO;

namespace ProductBuyerService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<ConsultProductDAO> ();
        }
    }
}
