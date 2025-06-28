using ProductService.DAO;

namespace ProductService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<ConsultTagsDAO> ();
        }
    }
}
