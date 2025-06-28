using ProductSellerService.DAO;

namespace ProductSellerService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<ConsultProductDAO> ();
            services.AddScoped<EditProductDAO> ();
        }
    }
}
