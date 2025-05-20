using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test {
    public class ApiWebApplicationFactory : WebApplicationFactory<StartupBase> {
        protected override void ConfigureWebHost (IWebHostBuilder builder) {
            builder.UseEnvironment ("Testing");
            // Configura servicios adicionales si es necesario
        }
    }
}

