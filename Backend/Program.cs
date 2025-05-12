using Backend.Config;

WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);
//TODO Implement BCrypt

// Add builder.Services to the container.
builder.Services.ConfigureBuilder (builder: builder);
builder.Services.ConfigureAuth (builder: builder);
builder.Services.AddAplicationDAOs ();
builder.Services.AddAplicationServices ();
builder.Services.AddUtils ();



WebApplication? app = builder.Build ();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ()) {
    app.MapOpenApi ();
    app.UseDeveloperExceptionPage ();
} else {
    app.UseExceptionHandler ("/Home/Error");
    app.UseHsts ();
}

// Middleware in correct orden: Routing -> CORS -> Auth -> Controllers.
app.UseHttpsRedirection ();                     // Redirige automáticamente cualquier petición HTTP a HTTPS.
app.UseRouting ();                              // Activa el middleware que permite enrutar las solicitudes entrantes
app.UseCors ("FromFrontend");         // Sirve para permitir o restringir solicitudes desde otros dominios 
app.UseAuthentication ();
app.UseAuthorization ();                        // Activa el middleware que revisa las políticas de autorización, como [Authorize].
app.MapControllers ();                          // Habilita que se puedan mapear los endpoints de controladores con atributos [HttpGet], [Route], etc. Necesario si usas API con controladores.
app.Run ();
