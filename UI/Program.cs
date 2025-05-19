using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Claims;
using System.Text;

DotNetEnv.Env.Load ();

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);
string backendUrl = builder.Configuration["BackendSettings:BackendUrl"] ?? "https://localhost:5001";

CultureInfo currentCulture = new CultureInfo ("es-MX");
CultureInfo.DefaultThreadCurrentCulture = currentCulture;
CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

// Add services to the container.
builder.Services.AddHttpClient ();
builder.Services.AddRazorPages ();
builder.Services.AddSingleton (new HttpClient ());
builder.Services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie (options => {
        options.Cookie.Name = "jwtToken"; // El nombre de la cookie
        options.Cookie.HttpOnly = true;   // Importante para seguridad (evita acceso JS)
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Usa HTTPS

        // Redirecciones automáticas si el usuario no está autenticado
        options.LoginPath = "/Login"; // Página de login
        options.AccessDeniedPath = "/AccessDenied"; // Página de acceso denegado

        // Tiempo de expiración de la cookie (persistencia de sesión)
        options.ExpireTimeSpan = TimeSpan.FromDays (7);
        options.SlidingExpiration = true; // Renovar cookie si el usuario sigue activo
    });
builder.Services.AddAuthorization ();
builder.Services.AddControllers (); // Para API


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
app.MapControllers ();
app.MapRazorPages ();

app.Run ();
