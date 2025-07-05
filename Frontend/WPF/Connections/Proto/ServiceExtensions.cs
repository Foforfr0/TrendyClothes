using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace WpfApp.Connections.Proto {
    public static class ServiceExtensions {
        public static void ConfiguregRPC (this IServiceCollection services, IConfiguration configuration) {
            string gRPCServer = "http://localhost:5109";
            //"http://grpcimageservice";

            services.AddGrpcClient<GetImageProduct.GetImageService.GetImageServiceClient> (options => {
                options.Address = new Uri (gRPCServer);
            })
            .ConfigurePrimaryHttpMessageHandler (() => {
                return new HttpClientHandler {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });

            services.AddGrpcClient<SaveImageProduct.SaveImageService.SaveImageServiceClient> (options => {
                options.Address = new Uri (gRPCServer);
            })
            .ConfigurePrimaryHttpMessageHandler (() => {
                return new HttpClientHandler {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });

            services.AddGrpcClient<SaveImageNewProduct.SaveNewImageService.SaveNewImageServiceClient> (options => {
                options.Address = new Uri (gRPCServer);
            })
            .ConfigurePrimaryHttpMessageHandler (() => {
                return new HttpClientHandler {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });
        }
    }

}
