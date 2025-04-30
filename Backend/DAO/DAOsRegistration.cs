using Backend.DAO.User.Auth;

namespace Backend.DAO {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<AuthDAO> ();
        }
    }
}
