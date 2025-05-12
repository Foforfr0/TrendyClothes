using Backend.Utils;

namespace Backend.Config {
    public static class UtilsConfiguration {
        public static void AddUtils (this IServiceCollection services) {
            services.AddScoped<ManageEmail> ();
        }
    }
}
