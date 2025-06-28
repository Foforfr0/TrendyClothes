using ClienteMAUI.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ClienteMAUI.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        lblError.IsVisible = false;

        if (!CamposRequeridosCompletos() ||
            !CorreoValido() ||
            !TelefonoValido() ||
            !PasswordSegura() ||
            !PasswordsCoinciden())
        {
            lblError.IsVisible = true;
            return;
        }

        var registerData = new RegisterRequest
        {
            FirstName = txtFirstName.Text.Trim(),
            LastName = txtLastName.Text.Trim(),
            MiddleName = txtMiddleName.Text.Trim(),
            Username = txtUsername.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            AreaCode = txtAreaCode.Text.Trim(),
            PhoneNumber = txtPhoneNumber.Text.Trim(),
            Password = txtPassword.Text.Trim()
        };
        try
        {
            using var testClient = new HttpClient();
            var pingResponse = await testClient.GetAsync("http://10.0.2.2:5003"); // prueba simple
            Console.WriteLine($"[PING] Status: {pingResponse.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PING] Error de conexión: {ex.Message}");
        }


        try
        {
            using var httpClient = new HttpClient();
            var url = "http://10.0.2.2:5003/api/User/Registration"; // o la IP de tu contenedor
            var response = await httpClient.PostAsJsonAsync(url, registerData);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Registro exitoso", "Tu cuenta ha sido creada. Inicia sesión para continuar.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var errores = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
                lblError.Text = "Error: " + errores?["title"] ?? "No se pudo registrar.";
                lblError.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = $"Error al conectar con el servidor: {ex.Message}";
            lblError.IsVisible = true;
        }
    }

    private bool CamposRequeridosCompletos()
    {
        if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
            string.IsNullOrWhiteSpace(txtLastName.Text) ||
            string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtAreaCode.Text) ||
            string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
        {
            lblError.Text = "Todos los campos obligatorios deben llenarse.";
            return false;
        }
        return true;
    }

    private bool CorreoValido()
    {
        string email = txtEmail.Text.Trim();
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            lblError.Text = "El correo electrónico no tiene un formato válido.";
            return false;
        }
        return true;
    }

    private bool TelefonoValido()
    {
        string areaCode = txtAreaCode.Text.Trim();
        string phone = txtPhoneNumber.Text.Trim();
        if (!Regex.IsMatch(areaCode, @"^\d{2,5}$") || !Regex.IsMatch(phone, @"^\d{7,10}$"))
        {
            lblError.Text = "El número telefónico o la lada no es válido.";
            return false;
        }
        return true;
    }

    private bool PasswordSegura()
    {
        if (txtPassword.Text.Length < 6)
        {
            lblError.Text = "La contraseña debe tener al menos 6 caracteres.";
            return false;
        }
        return true;
    }

    private bool PasswordsCoinciden()
    {
        if (txtPassword.Text != txtConfirmPassword.Text)
        {
            lblError.Text = "Las contraseñas no coinciden.";
            return false;
        }
        return true;
    }
}
