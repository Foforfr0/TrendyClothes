using AuctionStatistics.DAO;

namespace AuctionStatistics.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<StatisticsDAO> ();
        }
    }
}
