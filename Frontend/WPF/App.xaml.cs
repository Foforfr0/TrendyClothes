using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;
using WebPage.Connections;
using WpfApp.Connections;
using WpfApp.Pages.User.Auth;
using WpfApp.Services.User.Auth;

namespace WpfApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static IServiceProvider? Services {
            get; private set;
        }

        protected override void OnStartup (StartupEventArgs e) {
            base.OnStartup (e);

            ServiceCollection services = new ServiceCollection ();

            IConfigurationRoot? config = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json", optional: false, reloadOnChange: true)
                .Build ();

            services.Configure<ServicesConfig> (config.GetSection ("Services"));

            ServiceProvider? tempProvider = services.BuildServiceProvider ();
            ServicesConfig? options = tempProvider.GetRequiredService<IOptions<ServicesConfig>>().Value;

            // 3. Registrar clientes REST
            Dictionary<string, string>? allNamedClients = new Dictionary<string, string>
            {
                // User - Auth
                { "Auth", options.REST.User.Auth.BaseUrl },

                // User - Profile
                { "Profile", options.REST.User.Profile.BaseUrl },

                // User - Account
                { "Account", options.REST.User.Account.BaseUrl },

                // Product
                { "Product", options.REST.Product.Product.BaseUrl },
                { "Buyer", options.REST.Product.Buyer.BaseUrl },
                { "Seller", options.REST.Product.Seller.BaseUrl },

                // Auction
                { "Auctioneer", options.REST.Auction.Auctioneer.BaseUrl },
                { "Participant", options.REST.Auction.Participant.BaseUrl },

                // gRPC
                { "Grpc", options.gRPC.BaseUrl }
            };

            foreach (KeyValuePair<string, string> entry in allNamedClients) {
                string? name = entry.Key;
                string? baseUrl = entry.Value;

                services.AddHttpClient (name, client => {
                    client.BaseAddress = new Uri (baseUrl);
                    client.DefaultRequestHeaders.Add ("Accept", "application/json");
                })
                .ConfigurePrimaryHttpMessageHandler (() => new HttpClientHandler {
                    UseCookies = true,
                    CookieContainer = new CookieContainer ()
                });
            }

            // 4. Agrega una fábrica reutilizable
            services.AddSingleton<HttpClientFactoryService> ();
            services.AddTransient<LoginService> ();
            Services = services.BuildServiceProvider ();
            var signInWindow = new SignInWindow (App.Services.GetRequiredService<LoginService> ());
            signInWindow.Show ();
        }
    }
}
