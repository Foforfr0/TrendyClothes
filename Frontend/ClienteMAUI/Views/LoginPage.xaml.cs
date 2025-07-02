using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auth;
using ClienteMAUI.Session;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ClienteMAUI.Views
{
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            SetEmailPanelEnabled(false);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            lblError.IsVisible = false;

            if (!AreLoginFieldsValid())
                return;

            var loginDto = new LoginDTO
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim()
            };

            try
            {
                using var httpClient = new HttpClient();
                var url = AuthEndpoints.ValidateCredentials;
                var response = await httpClient.PostAsJsonAsync(url, loginDto);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Credenciales correctas", "Ahora valida tu correo y solicita el código.", "OK");
                    SetEmailPanelEnabled(true);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    lblError.Text = $"Error: {content}";
                    lblError.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error al conectar: {ex.Message}";
                lblError.IsVisible = true;
            }
        }


        private async void OnSendCodeClicked(object sender, EventArgs e)
        {
            string username = txtUsername.Text?.Trim() ?? "";
            string email = txtEmail.Text?.Trim() ?? "";

            if (!IsEmailValid(email) || string.IsNullOrWhiteSpace(username))
            {
                ShowError("Por favor, completa el usuario y un correo válido.");
                return;
            }

            try
            {
                using var httpClient = new HttpClient();

                // Crear y enviar código 2FA directamente
                var sendCodeUrl = AuthEndpoints.CreateTwoFactorCode;
                var body = new { username, email };
                var codeResponse = await httpClient.PatchAsync(sendCodeUrl, JsonContent.Create(body));

                if (codeResponse.IsSuccessStatusCode)
                {
                    await DisplayAlert("Código enviado", "Revisa tu correo para continuar.", "OK");
                }
                else
                {
                    var error = await codeResponse.Content.ReadAsStringAsync();
                    ShowError("Error al enviar código: " + error);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error al conectar con el servidor: {ex.Message}");
            }
        }




        private async void OnValidateCodeClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                ShowError("El código no puede estar vacío.");
                return;
            }

            var dto = new CodeTwoFactorDTO
            {
                Username = txtUsername.Text.Trim(),
                TwoFactorCode = txtCode.Text.Trim()
            };

            try
            {
                using var httpClient = new HttpClient();
                var url = AuthEndpoints.ValidateTwoFactorCode;
                var response = await httpClient.PostAsJsonAsync(url, dto);

                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resultJson);
                    var result = JsonSerializer.Deserialize<TwoFactorResponseDTO>(resultJson);

                    if (result != null)
                    {
                        // Guardar en preferencias para persistencia
                        Preferences.Set("jwtToken", result.JwtToken);
                        Preferences.Set("username", dto.Username);

                        // Guardar en singleton para uso en runtime
                        UserSession.Instance.SetUser(dto.Username, result.JwtToken);
                    }

                    await Navigation.PushAsync(new MainMenuPage());
                }

            }
            catch (Exception ex)
            {
                ShowError($"Error de conexión: {ex.Message}");
            }
        }


        private async void OnRegsiterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        // ========== MÉTODOS DE VALIDACIÓN ==========

        private bool AreLoginFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("El nombre de usuario es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("La contraseña es obligatoria.");
                return false;
            }

            return true;
        }

        
        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.IsVisible = true;
        }

        private void SetEmailPanelEnabled(bool isEnabled)
        {
            txtEmail.IsEnabled = isEnabled;
            txtCode.IsEnabled = isEnabled;

            foreach (var view in ((Grid)txtEmail.Parent.Parent).Children)
            {
                switch (view)
                {
                    case Button btn:
                        btn.IsEnabled = isEnabled;
                        break;
                    case Entry entry:
                        entry.IsEnabled = isEnabled;
                        break;
                }
            }

        }
    }
}
