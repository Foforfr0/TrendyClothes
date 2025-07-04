using AuctionParticipantService.DAO;


namespace AuctionParticipantService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IAuctionService, AuctionService> ();
        }
    }
}