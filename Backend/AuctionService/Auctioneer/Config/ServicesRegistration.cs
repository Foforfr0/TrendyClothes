using AuctionAuctioneerService.Services.Implements;
using AuctionAuctioneerService.Services.Intefaces;

namespace AuctionAuctioneerService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IConsultAuctionService, ConsultAuctionService> ();
            services.AddScoped<ICreateAuctionService, CreateAuctionService> ();
            services.AddScoped<IUpdateAuctionService, UpdateAuctionService> ();
            services.AddScoped<IConsultImagesService, ConsultImagesService> ();
            services.AddScoped<IConsultTagsService, ConsultTagsService> ();
        }
    }
}