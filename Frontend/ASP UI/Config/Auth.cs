namespace WebPage.Config {
    public static class Auth {
        public static void ConfigureAuth (this IServiceCollection services, WebApplicationBuilder builder) {
            builder.Services.AddAuthentication (options => {
                options.DefaultScheme = "signInScheme";
                options.DefaultAuthenticateScheme = "signInScheme";
                options.DefaultChallengeScheme = "signInScheme";
                options.DefaultSignInScheme = "signInScheme";
            })
                .AddCookie ("signInScheme", options => {
                    options.Cookie.Name = "signInCookie";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.LoginPath = "/User/Auth/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays (7);
                    options.SlidingExpiration = true;
                })
                .AddCookie ("jwtScheme", options => {
                    options.Cookie.Name = "jwtToken"; // Cookie usada SOLO por backend si decides hacerlo (aunque aquí no es necesaria si tú la pones manualmente)
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.LoginPath = "/User/Auth/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays (7);
                    options.SlidingExpiration = true;
                });
            builder.Services.AddAuthorization ();
        }
    }
}
