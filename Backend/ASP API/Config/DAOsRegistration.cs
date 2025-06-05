using Backend.DAO.Auction;
using Backend.DAO.Product;
using Backend.DAO.User;

namespace Backend.Config {
    public static class DAOsRegistration {
        public static void AddAplicationDAOs (this IServiceCollection services) {
            // Auction DAOs
            services.AddScoped<ConsultAuctionDAO> ();
            services.AddScoped<CreateAuctionDAO> ();
            services.AddScoped<UpdateAuctionDAO> ();

            // ProductController DAOs
            services.AddScoped<ConsultProductDAO> ();
            services.AddScoped<ConsultTagsDAO> ();
            services.AddScoped<EditProductDAO> ();

            // User DAOs
            services.AddScoped<AuthDAO> ();
            services.AddScoped<ConsultUserDAO> ();
            services.AddScoped<ProfileDAO> ();
            services.AddScoped<RegistrationDAO> ();
        }
    }
}
