using System.Globalization;

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
