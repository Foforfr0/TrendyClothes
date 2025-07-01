using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WebPage.Connections;
using WebPage.Connections.REST.User.Auth;
using WpfApp.Connections;
using WpfApp.DTO;
using WpfApp.DTO.User.Auth;

namespace WpfApp.Services.User.Auth {
    public class LoginService {
        private readonly HttpClient _httpClient;
        private readonly UserAuthConfig _authConfig;

        public LoginService (HttpClientFactoryService clientFactory, IOptions<ServicesConfig> config) {
            _httpClient = clientFactory.GetClient ("Auth");
            _authConfig = config.Value.REST.User.Auth;
        }

        public async Task<MessageResponse<HttpStatusCode>> LoginAsync (string username, string password) {
            try {
                var response = await _httpClient.PostAsJsonAsync (
                    _authConfig.Login.Login,
                    new {
                        username, password
                    });

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginDTO>> ();

                return response.StatusCode switch {
                    HttpStatusCode.OK => MessageResponse<HttpStatusCode>.Success (result?.message ?? "OK", HttpStatusCode.OK),
                    HttpStatusCode.BadRequest => MessageResponse<HttpStatusCode>.Success ("Correo electrónico requerido.", HttpStatusCode.BadRequest),
                    HttpStatusCode.NotFound => MessageResponse<HttpStatusCode>.Success (result?.message ?? "No encontrado", HttpStatusCode.Unauthorized),
                    _ => MessageResponse<HttpStatusCode>.Failure ("Error interno del servidor: " + (result?.message ?? "Error del servidor."), HttpStatusCode.InternalServerError)
                };
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al iniciar sesión: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse<HttpStatusCode>> ValidateEmailUserAsync (string username, string email) {
            try {
                var url = $"{_authConfig.Login.ValidateEmailUser}?username={Uri.EscapeDataString (username)}&email={Uri.EscapeDataString (email)}";

                var response = await _httpClient.GetAsync (url);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<EmailDTO>> ();

                return response.StatusCode switch {
                    HttpStatusCode.OK => MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.OK),
                    HttpStatusCode.BadRequest => MessageResponse<HttpStatusCode>.Success ("Correo electrónico requerido.", HttpStatusCode.BadRequest),
                    HttpStatusCode.NotFound => MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.Unauthorized),
                    _ => MessageResponse<HttpStatusCode>.Success ("Error interno del servidor: " + result.message, HttpStatusCode.InternalServerError)
                };
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al validar email: " + ex.ToString (), HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse<HttpStatusCode>> CreateTwoFactorCodeAsync (string username, string email) {
            try {
                var response = await _httpClient.PatchAsJsonAsync (
                    _authConfig.Login.PostTwoFactorCode,
                    new {
                        username, email
                    });

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>> ();

                return response.StatusCode switch {
                    HttpStatusCode.OK => MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.OK),
                    HttpStatusCode.BadRequest => MessageResponse<HttpStatusCode>.Success ("Usuario o código inválido.", HttpStatusCode.BadRequest),
                    HttpStatusCode.NotFound => MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.Unauthorized),
                    _ => MessageResponse<HttpStatusCode>.Success ("Error interno del servidor.", HttpStatusCode.InternalServerError)
                };
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al crear código 2FA: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse<HttpStatusCode>> ValidateTwoFactorCodeAsync (string username, string twoFactorCode) {
            try {
                var response = await _httpClient.PostAsJsonAsync (
                    _authConfig.Login.ValidateTwoFactorCode,
                    new {
                        username, twoFactorCode
                    });

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>> ();

                return response.StatusCode switch {
                    HttpStatusCode.OK => MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.OK),
                    HttpStatusCode.BadRequest => MessageResponse<HttpStatusCode>.Success ("Código inválido.", HttpStatusCode.BadRequest),
                    HttpStatusCode.NotFound => MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.Unauthorized),
                    _ => MessageResponse<HttpStatusCode>.Success ("Error interno del servidor.", HttpStatusCode.InternalServerError)
                };
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al validar código 2FA: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteTwoFactorCodeAsync (string username) {
            var url = $"{_authConfig.Login.DeleteTwoFactorCode}?username={Uri.EscapeDataString (username)}";
            await _httpClient.DeleteAsync (url);
        }
    }
}
