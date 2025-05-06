using Backend.DTO;
using Backend.DTO.User.Auth;
using Backend.DTO.User.Profile;

namespace Backend.Config {
    public static class DTOsRegistration {
        public static void AddAplicationDTOs (this IServiceCollection services) {
            // DTO
            services.AddScoped<MessageResponse<object>> ();

            // DTO.User.Auth
            services.AddScoped<CodeTwoFactorDTO> ();
            services.AddScoped<EmailDTO> ();
            services.AddScoped<LoginDTO> ();

            // DTO.User.Profile
            services.AddScoped<ViewMyProfileDTO> ();
        }
    }
}
