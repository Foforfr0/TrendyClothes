using Backend.Auth;

namespace Backend.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services) {
            services.AddScoped<ManageJWTToken> ();
        }
    }
}