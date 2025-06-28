using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace ClienteMAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        private const string SimulatedCode = "123456"; // Código de prueba

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

            if (IsValidUser(txtUsername.Text, txtPassword.Text))
            {
                await DisplayAlert("Autenticación exitosa", "Usuario válido, ahora puedes verificar tu correo", "OK");
                SetEmailPanelEnabled(true); // Habilita lado derecho
            }
            else
            {
                ShowError("Usuario o contraseña incorrectos.");
            }
        }

        private async void OnSendCodeClicked(object sender, EventArgs e)
        {
            if (!IsEmailValid(txtEmail.Text))
            {
                ShowError("Por favor, ingresa un correo válido.");
                return;
            }

            await DisplayAlert("Código enviado", $"Tu código es: {SimulatedCode}", "OK");
        }

        private async void OnValidateCodeClicked(object sender, EventArgs e)
        {
            if (txtCode.Text?.Trim() == SimulatedCode)
            {
                await Navigation.PushAsync(new MainMenuPage());
            }
            else
            {
                ShowError("El código ingresado no es válido.");
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

        private bool IsValidUser(string username, string password)
        {
            return username == "testuser" && password == "123456";
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

            // También activa los botones relacionados si existen
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
