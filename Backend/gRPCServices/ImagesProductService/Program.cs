using ImagesProductService.DAO;
using ImagesProductService.Entities;
using ImagesProductService.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);

// === Add Services ===
builder.Services.AddGrpc ();
builder.Services.AddCors (options => {
    options.AddPolicy ("FromFrontend", policy => {
        policy.WithOrigins ("http://localhost:8081")
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

builder.WebHost.UseUrls ("http://+:80");

if (!builder.Environment.IsDevelopment ()) {
    builder.Services.AddDataProtection ()
    .PersistKeysToFileSystem (new DirectoryInfo ("/var/dpkeys"))
    .SetApplicationName ("TrendyClothes");
}

var app = builder.Build ();

if (app.Environment.IsDevelopment ()) {
    app.UseDeveloperExceptionPage ();
} else {
    app.UseExceptionHandler ("/Home/Error");
    app.UseHsts ();
}

// === Middleware ===
app.UseCors ("FromFrontend");

// Muestra información básica si acceden vía HTTP
app.MapGet ("/", () =>
    "Este servicio gRPC solo acepta conexiones a través de clientes gRPC. Visita: https://go.microsoft.com/fwlink/?linkid=2086909");

// === Mapear Servicios gRPC ===
app.MapGrpcService<GetImageServiceImpl> ();
app.MapGrpcService<SaveImageServiceImpl> ();

app.Run ();
