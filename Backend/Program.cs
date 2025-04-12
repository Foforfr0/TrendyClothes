using Backend.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);




// Add services to the container.
builder.Services.AddOpenApi ();             // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers ();
builder.Services.AddCors (options => {
    options.AddPolicy ("ExamplePolicyCors", policy => {
        // policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); // Permite cualquier conexión.
        policy.WithOrigins ("http://localhost:8080") // indica qué dominios pueden hacer peticiones.
              .AllowAnyHeader ()                            // permite cualquier encabezado (como JSON, tokens, etc).
              .AllowAnyMethod ();                           // permite cualquier método HTTP.
    });
});
builder.Services.AddControllers ();
builder.Services.AddDbContext<TrendyClothesDBContext> (options =>
    options.UseSqlServer (builder.Configuration.GetConnectionString ("DefaultConnection")));
builder.Services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie (options => {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });





WebApplication? app = builder.Build ();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ()) {
    app.MapOpenApi ();
}
if (app.Environment.IsDevelopment ()) {
    app.UseDeveloperExceptionPage ();
} else {
    app.UseExceptionHandler ("/Home/Error");
    app.UseHsts ();
}




app.UseHttpsRedirection ();
app.UseAuthorization ();
app.UseCors ("ExamplePolicyCors");
app.MapControllers ();
app.MapGet ("/", () => { }).WithName ("Home");
app.MapControllerRoute (    //Define las rutas predeterminadas para los controladores de la aplicación
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run ();
