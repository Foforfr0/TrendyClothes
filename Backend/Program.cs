using Backend.Config;

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);
//TODO Implement BCrypt

// Add services to the container.
builder.Services.ConfigureBuilder (builder);
builder.Services.AddAplicationDTOs ();
builder.Services.AddAplicationDAOs ();
builder.Services.AddAplicationServices ();
builder.Services.ConfigureAuth ();





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
