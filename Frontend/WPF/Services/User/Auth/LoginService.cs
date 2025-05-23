using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WpfApp.DTO;
using WpfApp.DTO.User.Auth;

namespace WpfApp.Services.User.Auth {
    public class LoginService {
        private HttpClient _httpClient;

        public LoginService () {
            _httpClient = new HttpClient { };
        }

        public async Task<MessageResponse<HttpStatusCode>> LoginAsync (string username, string password) {
            try {
                HttpResponseMessage? response = await _httpClient.PostAsJsonAsync (
                    "https://localhost:5001/api/User/Login",
                    new {
                        username, password
                    });

                ApiResponse<LoginDTO>? result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginDTO>> ();

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
                HttpResponseMessage response = await _httpClient.GetAsync (
                    $"https://localhost:5001/api/User/Login/ValidateEmailUser?username={Uri.EscapeDataString(username)}&email={Uri.EscapeDataString (email)}");

                ApiResponse<EmailDTO>? result = await response.Content.ReadFromJsonAsync<ApiResponse<EmailDTO>> ();

                if (response.StatusCode == HttpStatusCode.OK) 
                    return MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.OK);
                
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return MessageResponse<HttpStatusCode>.Success ("Correo electrónico requerido.", HttpStatusCode.BadRequest);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.Unauthorized);

                else
                    return MessageResponse<HttpStatusCode>.Success ("Error interno del servidor: " + result.message, HttpStatusCode.InternalServerError);
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al iniciar sesión: " + ex.ToString (), HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse<HttpStatusCode>> CreateTwoFactorCodeAsync (string username, string email) {
            try {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync (
                    $"https://localhost:5001/api/User/Login/CreateTwoFactorCode",
                    new {
                        username, email
                    });

                ApiResponse<string>? result = await response.Content.ReadFromJsonAsync<ApiResponse<string>> ();

                if (response.StatusCode == HttpStatusCode.OK)
                    return MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.OK);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return MessageResponse<HttpStatusCode>.Success ("Usuario o twoFactorCode inválidos.", HttpStatusCode.BadRequest);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.Unauthorized);

                else
                    return MessageResponse<HttpStatusCode>.Success ("Error interno del servidor.", HttpStatusCode.InternalServerError);
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al iniciar sesión: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse<HttpStatusCode>> ValidateTwoFactorCodeAsync (string username, string twoFactorCode) {
            try {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync (
                    $"https://localhost:5001/api/User/Login/ValidateTwoFactorCode",
                    new {
                        username, twoFactorCode
                    });

                ApiResponse<string>? result = await response.Content.ReadFromJsonAsync<ApiResponse<string>> ();

                if (response.StatusCode == HttpStatusCode.OK)
                    return MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.OK);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return MessageResponse<HttpStatusCode>.Success ("Usuario o twoFactorCode inválidos.", HttpStatusCode.BadRequest);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return MessageResponse<HttpStatusCode>.Success (result.message, HttpStatusCode.Unauthorized);

                else
                    return MessageResponse<HttpStatusCode>.Success ("Error interno del servidor.", HttpStatusCode.InternalServerError);
            } catch (Exception ex) {
                return MessageResponse<HttpStatusCode>.Failure ("Excepción al iniciar sesión: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteTwoFactorCodeAsync (string username) {
            HttpResponseMessage response = await _httpClient.DeleteAsync (
                $"https://localhost:5001/api/User/Login/ValidateTwoFactorCode?username={Uri.EscapeDataString (username)}");
        }
    }
}
