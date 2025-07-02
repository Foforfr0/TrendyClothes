namespace WebPage.Config {
    public static class gRPC {
        public static void ConfiguregRPC (this IServiceCollection services, WebApplicationBuilder builder) {
            string gRPCServer = builder.Configuration["Services:REST:gRPC:BaseUrl"] ?? "http://grpcimageservice";

            builder.Services.AddGrpcClient<GetImageProduct.GetImageService.GetImageServiceClient> (options => {
                options.Address = new Uri (gRPCServer); // Dirección del servidor gRPC
            })
                .ConfigurePrimaryHttpMessageHandler (() => {
                    return new HttpClientHandler {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });

            builder.Services.AddGrpcClient<SaveImageProduct.SaveImageService.SaveImageServiceClient> (options => {
                options.Address = new Uri (gRPCServer); // Mismo servidor o diferente, si aplica
            })
                .ConfigurePrimaryHttpMessageHandler (() => {
                    return new HttpClientHandler {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });
            builder.Services.AddGrpcClient<SaveImageNewProduct.SaveNewImageService.SaveNewImageServiceClient> (options => {
                options.Address = new Uri (gRPCServer); // Mismo servidor o diferente, si aplica
            })
                .ConfigurePrimaryHttpMessageHandler (() => {
                    return new HttpClientHandler {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });
        }
    }
}
