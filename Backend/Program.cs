using Backend.DAO;
using Backend.Entities;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);
//TODO Implement BCrypt




// Add services to the container.
builder.Services.AddOpenApi ();             // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers ();
builder.Services.AddControllers ()
    .AddJsonOptions (options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddCors (options => {
    options.AddPolicy ("FromFrontend", policy => {
        // policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); // Permite cualquier conexión.
        policy.WithOrigins ("https://localhost:8081") // indica qué dominios pueden hacer peticiones.
              .AllowAnyHeader ()                            // permite cualquier encabezado (como JSON, tokens, etc).
              .AllowAnyMethod ();                           // permite cualquier método HTTP.
    });
});
builder.Services.AddDbContext<TrendyClothesDBContext> (options =>
    options.UseSqlServer (
        builder.Configuration.GetConnectionString ("SQLServer")),
    ServiceLifetime.Scoped);
builder.Services.AddAuthentication ("Bearer")
    .AddJwtBearer ("Bearer", options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //TODO
            ValidIssuer = "tu-emisor",
            ValidAudience = "tu-audiencia",
            IssuerSigningKey = new SymmetricSecurityKey (
                Encoding.UTF8.GetBytes ("clave-secreta-suficientemente-larga"))
        };
    });




// Add business services 
builder.Services.AddAplicationDAOs ();
builder.Services.AddAplicationServices ();




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




app.UseHttpsRedirection ();                     // Redirige automáticamente cualquier petición HTTP a HTTPS.
app.UseRouting ();                              // Activa el middleware que permite enrutar las solicitudes entrantes
app.UseCors ("FromFrontend");         // Sirve para permitir o restringir solicitudes desde otros dominios 
app.UseAuthentication ();
app.UseAuthorization ();                        // Activa el middleware que revisa las políticas de autorización, como [Authorize].
/**
 * app.MapDefaultControllerRoute ();               // Define una ruta básica estilo MVC.
 * app.MapControllerRoute (                        // Define las rutas predeterminadas para los controladores de la aplicación
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 * app.MapGet (                                    // Agrega una ruta tipo Minimal API que responde a GET /. 
 *      "/", 
 *      () => { }).WithName ("Home");
 */
app.MapControllers ();                          // Habilita que se puedan mapear los endpoints de controladores con atributos [HttpGet], [Route], etc. Necesario si usas API con controladores.
app.Run ();
