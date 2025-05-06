using Backend.DAO.User;

namespace Backend.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            // DAO.User
            services.AddScoped<UserDAO> ();
            services.AddScoped<AuthDAO> ();
            services.AddScoped<ProfileDAO> ();
        }
    }
}
