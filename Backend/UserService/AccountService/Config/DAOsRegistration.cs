using AccountService.DAO;

namespace AccountService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<ConsultUserDAO> ();
            services.AddScoped<RegistrationDAO> ();
        }
    }
}
