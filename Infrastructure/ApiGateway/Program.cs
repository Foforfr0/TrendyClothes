WebApplicationBuilder? builder = WebApplication.CreateBuilder (args);

builder.Services.AddReverseProxy ()
    .LoadFromConfig (builder.Configuration.GetSection ("ReverseProxy"));
builder.Services.AddCors (options => {
    options.AddPolicy ("FromFrontend", policy => {
        policy.WithOrigins ("http://localhost:8081")
            .AllowCredentials ()                              // Necesary for Cookies.
            .AllowAnyHeader ()                                // Allow any header (JSON, tokens, etc).
            .AllowAnyMethod ();                               // Allow any HTTP method.
    });
});

WebApplication? app = builder.Build ();

app.MapReverseProxy ();

app.UseCors ("FromFrontend");

app.Use (async (context, next) => {
    if (context.Request.Method == HttpMethods.Options) {
        context.Response.StatusCode = 200;
        return;
    }

    await next ();
});

app.Run ();
