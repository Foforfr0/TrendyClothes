using AuctionParticipantService.DAO;

namespace AuctionParticipantService.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            services.AddScoped<ConsultAuctionDAO> ();
            services.AddScoped<ConsultUserDAO> ();
            services.AddScoped<CreateAuctionDAO> ();
            services.AddScoped<UpdateAuctionDAO> ();
        }
    }
}
