using System.Globalization;

DotNetEnv.Env.Load ();
string backendUrl = Environment.GetEnvironmentVariable ("BACKEND_URL") ?? "";

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);

CultureInfo currentCulture = new CultureInfo ("es-MX");
CultureInfo.DefaultThreadCurrentCulture = currentCulture;
CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

// Add services to the container.
builder.Services.AddRazorPages ();
builder.Services.AddControllers ();

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


app.UseHttpsRedirection ();
app.UseRouting ();
app.UseAuthorization ();
app.MapStaticAssets ();
app.MapRazorPages ();
app.MapRazorPages ()
   .WithStaticAssets ();
app.MapControllers ();
//app.MapFallbackToPage ("/home");

app.Run ();
