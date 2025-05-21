using Backend.DAO.User;

namespace Backend.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<UserDAO> ();
            services.AddScoped<AuthDAO> ();
            services.AddScoped<ProfileDAO> ();
            services.AddScoped<RegistrationDAO> ();
        }
    }
}
