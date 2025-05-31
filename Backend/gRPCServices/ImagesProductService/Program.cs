using ImagesProductService.DAO;
using ImagesProductService.Entities;
using ImagesProductService.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);

// === Add Services ===
builder.Services.AddGrpc ();
builder.Services.AddCors (options => {
    options.AddPolicy ("FromFrontend", policy => {
        policy.WithOrigins ("https://localhost:8081")
              .AllowCredentials ()
              .AllowAnyHeader ()
              .AllowAnyMethod ();
    });
});

// Validación básica de la cadena de conexión
string? connectionString = builder.Configuration.GetConnectionString ("SQLServer");
if (string.IsNullOrEmpty (connectionString)) {
    throw new InvalidOperationException ("La cadena de conexión 'SQLServer' no está configurada.");
}

builder.Services.AddScoped<ImageProductDAO> ();

builder.Services.AddDbContext<TrendyClothesDBContext> (options =>
    options.UseSqlServer (connectionString),
    ServiceLifetime.Scoped);

var app = builder.Build ();

// === Middleware ===
app.UseHttpsRedirection ();
app.UseCors ("FromFrontend");

// Muestra información básica si acceden vía HTTP
app.MapGet ("/", () =>
    "Este servicio gRPC solo acepta conexiones a través de clientes gRPC. Visita: https://go.microsoft.com/fwlink/?linkid=2086909");

// === Mapear Servicios gRPC ===
app.MapGrpcService<ImageProductServiceImpl> ();

app.Run ();
