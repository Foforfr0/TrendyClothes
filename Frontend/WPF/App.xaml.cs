using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using WpfApp.Connections.Proto;
using WpfApp.Pages;

namespace WpfApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static IServiceProvider Services {
            get; private set;
        }

        private IHost _host;

        public App () {
            _host = Host.CreateDefaultBuilder ()
                .ConfigureAppConfiguration ((context, config) => {
                    config.AddJsonFile ("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices ((context, services) => {
                    services.ConfiguregRPC (context.Configuration);
                    // Agrega más servicios si es necesario, como ViewModels o helpers
                    services.AddSingleton<WindowContainer> ();
                })
                .Build ();

            Services = _host.Services;
        }

        protected override async void OnStartup (StartupEventArgs e) {
            await _host.StartAsync ();
            base.OnStartup (e);

            var mainWindow = Services.GetRequiredService<WindowContainer> ();
            mainWindow.Show ();
        }

        protected override async void OnExit (ExitEventArgs e) {
            await _host.StopAsync ();
            _host.Dispose ();
            base.OnExit (e);
        }
    }
}
