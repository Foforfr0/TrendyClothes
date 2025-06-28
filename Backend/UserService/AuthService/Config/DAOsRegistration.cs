using AuthService.DAO;

namespace AuthService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<AuthDAO> ();
            services.AddScoped<ConsultUserDAO> ();
        }
    }
}
