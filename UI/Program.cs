using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

DotNetEnv.Env.Load ();

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);
string backendUrl = builder.Configuration["BackendSettings:BackendUrl"] ?? "https://localhost:5001";

CultureInfo currentCulture = new CultureInfo ("es-MX");
CultureInfo.DefaultThreadCurrentCulture = currentCulture;
CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

// Add services to the container.
builder.Services.AddHttpClient ();
builder.Services.AddSingleton (new HttpClient());
builder.Services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie (options => {
        options.LoginPath = "/User/Auth/Login";
        options.LogoutPath = "/User/Auth/Logout";
        options.AccessDeniedPath = "/User/Auth/AccessDenied";
        options.Cookie.Name = "jwtToken"; // Same name according with de backend cookie
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
