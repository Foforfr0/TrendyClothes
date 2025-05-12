using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

DotNetEnv.Env.Load ();
string backendUrl = Environment.GetEnvironmentVariable ("BACKEND_URL") ?? "https://localhost:5001";

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);

CultureInfo currentCulture = new CultureInfo ("es-MX");
CultureInfo.DefaultThreadCurrentCulture = currentCulture;
CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

// Add services to the container.
builder.Services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie (options => {
        options.LoginPath = "/User/Auth/Login";
        options.LogoutPath = "/User/Auth/Logout";
        options.AccessDeniedPath = "/User/Auth/AccessDenied";
        options.Cookie.Name = "jwt"; // Same name according with de backend cookie
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes (10080);
    });
builder.Services.AddAuthorization ();
builder.Services.AddRazorPages ();
builder.Services.AddControllers (); // Para API


WebApplication? app = builder.Build ();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment ()) {
    app.UseExceptionHandler ("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ();
}
app.Use (middleware: async (HttpContext context, Func<Task> next) => {
    context.Items["BackendUrl"] = backendUrl;
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
