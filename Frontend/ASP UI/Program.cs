using System.Globalization;
using WebPage.Connections;

DotNetEnv.Env.Load ();

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);
string backendUrl = builder.Configuration["BackendSettings:BackendUrl"] ?? "https://localhost:5001";
string gRPCServer = builder.Configuration["BackendSettings:gRPCServer"] ?? "https://localhost:5002";

CultureInfo currentCulture = new CultureInfo ("es-MX");
CultureInfo.DefaultThreadCurrentCulture = currentCulture;
CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

// Add services to the container.
builder.Services.Configure<ServicesConfig> (
    builder.Configuration.GetSection ("Services"));
builder.Services.AddSingleton<ServicesBuilder> ();
builder.Services.AddHttpClient ();
builder.Services.AddRazorPages ();
builder.Services.AddSingleton (new HttpClient ());
builder.Services.AddAuthentication (options => {
    options.DefaultScheme = "signInScheme";
    options.DefaultAuthenticateScheme = "signInScheme";
    options.DefaultChallengeScheme = "signInScheme";
    options.DefaultSignInScheme = "signInScheme";
})
    .AddCookie ("signInScheme", options => {
        options.Cookie.Name = "signInCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/User/Auth/Login";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays (7);
        options.SlidingExpiration = true;
    })
    .AddCookie ("jwtScheme", options => {
        options.Cookie.Name = "jwtToken"; // Cookie usada SOLO por backend si decides hacerlo (aunque aquí no es necesaria si tú la pones manualmente)
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/User/Auth/Login";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays (7);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization ();
builder.Services.AddControllers (); // Para API
builder.Services.AddGrpcClient<ImageProduct.ImageProductService.ImageProductServiceClient> (options => {
    options.Address = new Uri (gRPCServer); // Dirección de tu servidor gRPC
})
    .ConfigurePrimaryHttpMessageHandler (() => {
        return new HttpClientHandler {
            // Esto es para ignorar errores de certificado solo si usas HTTPS con un cert autofirmado (desarrollo).
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    });
builder.Logging.ClearProviders ();
builder.Logging.AddConsole (); // Esto es lo que imprime en consola
builder.Logging.SetMinimumLevel (LogLevel.Trace);



WebApplication? app = builder.Build ();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment ()) {
    app.UseExceptionHandler ("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ();
}
app.Use (middleware: async (HttpContext context, Func<Task> next) => {
    context.Items["BACKEND_URL"] = backendUrl;
    await next ();
});

// Middleware in correct orden: Routing -> CORS -> Auth -> Controllers.
app.UseHttpsRedirection ();
app.UseStaticFiles ();
app.UseRouting ();

app.UseAuthentication ();
app.UseAuthorization ();

// Solo usa estos:
app.MapRazorPages ();
app.MapControllers ();

app.Run ();
