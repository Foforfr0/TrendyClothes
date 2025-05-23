using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Windows;

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

            services.AddSingleton (new HttpClient ());
            services.AddHttpClient ("API_REST", client => {
                client.BaseAddress = new Uri ("https://localhost:5001");
                client.DefaultRequestHeaders.Add ("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler (() => new HttpClientHandler {
                UseCookies = true,
                CookieContainer = new CookieContainer ()
            });

            Services = services.BuildServiceProvider ();
        }
    }
}
