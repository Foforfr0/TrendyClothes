using AuctionStatistics.Services.Implements;
using AuctionStatistics.Services.Interfaces;

namespace AuctionStatistics.Config {
    public static class ServicesRegistration {
        public static void AddApplicationServices (this IServiceCollection services) {
            services.AddScoped<IStatisticsService, StatisticsService> ();
        }
    }
}