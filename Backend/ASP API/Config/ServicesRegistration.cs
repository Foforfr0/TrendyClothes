using Backend.Services.Implements.Auction;
using Backend.Services.Implements.Product;
using Backend.Services.Implements.User;
using Backend.Services.Intefaces.Auction;
using Backend.Services.Intefaces.Product;
using Backend.Services.Intefaces.User;

namespace Backend.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            // Auction Services
            services.AddScoped<IConsultAuctionService, ConsultAuctionService> ();
            services.AddScoped<ICreateAuctionService, CreateAuctionService> ();
            services.AddScoped<IUpdateAuctionService, UpdateAuctionService> ();

            // ProductController Services
            services.AddScoped<IConsultProductService, ConsultProductService> ();
            services.AddScoped<IConsultTagsService, ConsultTagsService> ();
            services.AddScoped<IEditProductService, EditProductService> ();

            // User Services
            services.AddScoped<IAuthService, AuthService> ();
            services.AddScoped<IConsultProfileService, ConsultProfileService> ();
            services.AddScoped<IDeleteAccountService, DeleteAccountService> ();
            services.AddScoped<IRegistrationAccountService, RegistrationAccountService> ();
            services.AddScoped<IValidateDataService, ValidateDataService> ();
        }
    }
}