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

        // Aquí iría el POST real
        await DisplayAlert("Registro exitoso", "Tu cuenta ha sido creada. Inicia sesión para continuar.", "OK");
        await Shell.Current.GoToAsync("..");
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
