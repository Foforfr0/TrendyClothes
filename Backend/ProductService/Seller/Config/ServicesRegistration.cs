using ProductSellerService.Services.Implements;
using ProductSellerService.Services.Intefaces;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Config {
    public static class ServicesRegistration {
        public static void AddAplicationServices (this IServiceCollection services) {
            services.AddScoped<IConsultProductService, ConsultProductService> ();
            services.AddScoped<IEditProductService, EditProductService> ();
            services.AddScoped<ICreateProductService, CreateProductService> ();
            services.AddScoped<IDeleteProductService, DeleteProductService> ();
        }
    }
}