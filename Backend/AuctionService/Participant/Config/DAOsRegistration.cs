using AuctionParticipantService.DAO;

namespace AuctionParticipantService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<AuctionDAO> ();

        }
    }
}
