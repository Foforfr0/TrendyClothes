using AuctionParticipantService.Services.Implements;
using AuctionParticipantService.Services.Intefaces;

namespace AuctionParticipantService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IConsultAuctionService, ConsultAuctionService> ();
            services.AddScoped<ICreateAuctionService, CreateAuctionService> ();
            services.AddScoped<IUpdateAuctionService, UpdateAuctionService> ();
        }
    }
}