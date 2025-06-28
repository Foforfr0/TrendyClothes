using Grpc.Core.Utils;
using Microsoft.AspNetCore.DataProtection;
using System.Globalization;
using System.Text.Json;
using WebPage.Config;
using WebPage.Connections;

DotNetEnv.Env.Load ();

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);

CultureInfo currentCulture = new CultureInfo ("es-MX");
CultureInfo.DefaultThreadCurrentCulture = currentCulture;
CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

// Add services to the container.
builder.WebHost.UseUrls ("http://+:80");
builder.Services.Configure<ServicesConfig> (
    builder.Configuration.GetSection ("Services"));
builder.Services.AddSingleton<ServicesBuilder> ();
builder.Services.AddHttpClient ();
builder.Services.AddRazorPages ()
    .AddJsonOptions (options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddSingleton (new HttpClient ());
Auth.ConfigureAuth (builder.Services, builder);
builder.Services.AddControllers (); // Para API
gRPC.ConfiguregRPC (builder.Services, builder);

builder.Logging.ClearProviders ();
builder.Logging.AddConsole (); // Esto es lo que imprime en consola
builder.Logging.SetMinimumLevel (LogLevel.Trace);

if (!builder.Environment.IsDevelopment ()) {
    builder.Services.AddDataProtection ()
    .PersistKeysToFileSystem (new DirectoryInfo ("/var/dpkeys"))
    .SetApplicationName ("TrendyClothes");
}

WebApplication? app = builder.Build ();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment ()) {
    app.UseExceptionHandler ("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ();
} else {
    app.UseHttpsRedirection ();
}
app.Use (middleware: async (HttpContext context, Func<Task> next) => {
    context.Items["BACKEND_URL"] = "http://localhost:5001";
    await next ();
});

// Middleware in correct orden: Routing -> CORS -> Auth -> Controllers.
app.UseStaticFiles ();
app.UseRouting ();

app.UseAuthentication ();
app.UseAuthorization ();

// Solo usa estos:
app.MapRazorPages ();
app.MapControllers ();

app.Run ();
