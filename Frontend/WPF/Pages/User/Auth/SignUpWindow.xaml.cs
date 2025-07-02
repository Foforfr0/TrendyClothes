using Microsoft.Win32;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WebPage.Connections;
using WpfApp.DTO.User.Register;
using WpfApp.Pages.Dialogs;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Auth
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
            SetInputFields();
            UpdateRegisterButtonState();
        }

        private void SetInputFields()
        {
            var validations = new (TextBox, string, int)[]
            {
                (TbFirstName, Utilities.Constants.NAMES_PATTERN, Utilities.Constants.MAX_LENGTH_NAMES),
                (TbMiddleName, Constants.NAMES_PATTERN, Constants.MAX_LENGTH_NAMES),
                (TbLastName, Utilities.Constants.NAMES_PATTERN, Utilities.Constants.MAX_LENGTH_NAMES),
                (TbPhoneNumber, Constants.NUMERIC_PATTERN, Constants.MAX_LENGTH_PHONENUMBER),
                (TbEmailAddress, Constants.EMAIL_ALLOWED_CHARS_PATTERN, Constants.MAX_LENGTH_EMAIL),
                (TbUsername, Constants.NAMES_PATTERN, Constants.MAX_LENGTH_USERNAME)
            };

            foreach (var (textBox, pattern, maxLength) in validations)
                InputUtilities.ValidateInput(textBox, pattern, maxLength);

            InputUtilities.ValidatePasswordInput(PbAccountPassword, Constants.MIN_LENGTH_PASSWORD,
                Utilities.Constants.MAX_LENGTH_PASSWORD);
            InputUtilities.ValidatePasswordInput(PbConfirmPassword, Constants.MIN_LENGTH_PASSWORD,
                Constants.MAX_LENGTH_PASSWORD);
            InputUtilities.ConvertToLowerCase(TbEmailAddress);
        }

        private string GetPassword()
        {
            return TbAccountPassword.Visibility == Visibility.Visible
                ? TbAccountPassword.Text
                : PbAccountPassword.Password;
        }

        private string GetConfirmedPassword()
        {
            return TbConfirmPassword.Visibility == Visibility.Visible
                ? TbConfirmPassword.Text
                : PbConfirmPassword.Password;
        }

        private void UpdateRegisterButtonState()
        {
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(TbFirstName.Text) &&
                !string.IsNullOrWhiteSpace(TbMiddleName.Text) &&
                !string.IsNullOrWhiteSpace(TbLastName.Text) &&
                !string.IsNullOrWhiteSpace(TbEmailAddress.Text) &&
                !string.IsNullOrWhiteSpace(TbPhoneNumber.Text) &&
                !string.IsNullOrWhiteSpace(GetPassword()) &&
                !string.IsNullOrWhiteSpace(GetConfirmedPassword());

            BtnRegisterUser.IsEnabled = allFieldsFilled;
        }

        private void RequiredFields_TextChanged(object sender, RoutedEventArgs e)
        {
            UpdateRegisterButtonState();
        }

        private void PasswordField_TextChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && PbAccountPassword.Password != textBox.Text)
                PbAccountPassword.Password = textBox.Text;
            UpdateRegisterButtonState();
        }

        private void PasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && TbAccountPassword.Text != passwordBox.Password)
                TbAccountPassword.Text = passwordBox.Password;
            UpdateRegisterButtonState();
        }

        private void ConfirmPasswordField_TextChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && PbConfirmPassword.Password != textBox.Text)
                PbConfirmPassword.Password = textBox.Text;
            UpdateRegisterButtonState();
        }

        private void ConfirmPasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && TbConfirmPassword.Text != passwordBox.Password)
                TbConfirmPassword.Text = passwordBox.Password;
            UpdateRegisterButtonState();
        }


        private void ChbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordUtilities.ShowPassword(TbAccountPassword, PbAccountPassword);
        }

        private void ChbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordUtilities.HidePassword(TbAccountPassword, PbAccountPassword);
        }

        private void ChbShowConfirmPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordUtilities.ShowPassword(TbConfirmPassword, PbConfirmPassword);
        }

        private void ChbShowConfirmPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordUtilities.HidePassword(TbConfirmPassword, PbConfirmPassword);
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void BtnRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            var dto = new RegisterDTO
            {
                FirstName = TbFirstName.Text.Trim(),
                MiddleName = TbMiddleName.Text.Trim(),
                LastName = TbLastName.Text.Trim(),
                Email = TbEmailAddress.Text.Trim(),
                PhoneNumber = TbPhoneNumber.Text.Trim(),
                AreaCode = "+52",
                Username = TbUsername.Text.Trim(),
                Password = GetPassword()
            };

            if (!InputUtilities.IsValidNameFormat(dto.FirstName))
            {
                MessageDialog.Show("Nombre inválido",
                    "El nombre debe iniciar con mayúscula y solo contener letras.",
                    AlertType.WARNING);
                return;
            }

            if (!InputUtilities.IsValidPasswordFormat(dto.Password))
            {
                MessageDialog.Show("Contraseña inválida",
                    "Debe tener una mayúscula, una minúscula, un número y un carácter especial.",
                    AlertType.WARNING);
                return;
            }


            try
            {
                var url = AuthEndpoints.RegisterUser;

                // Optional: log serialized DTO for debugging
                var jsonPayload = JsonSerializer.Serialize(dto);
                Console.WriteLine($"[DEBUG] Sending RegisterDTO: {jsonPayload}");

                var response = await HttpClientHelper.PostAsync(url, dto);

                if (response.IsSuccessStatusCode)
                {
                    MessageDialog.Show("RegUser_DialogTSuccess", "RegUser_DialogDSuccess", AlertType.SUCCESS);

                    var loginWindow = new SignInWindow();
                    loginWindow.Show();
                    this.Close();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"[DEBUG] Registration failed: {errorContent}");

                    MessageDialog.Show("RegUser_DialogTFailed", $"Registro fallido:\n{errorContent}", AlertType.ERROR);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] Register error: {ex}");
                MessageDialog.Show("RegUser_DialogTFailed", $"Error de conexión: {ex.Message}", AlertType.ERROR);
            }
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var container = new WindowContainer();
            container.Show();
            this.Close();
        }

    }
}
