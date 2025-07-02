using System.Net;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Pages.Dialogs;
using WpfApp.DTO;
using WpfApp.Services.User.Auth;
using WpfApp.Utilities;
using WpfApp.DTO.User.Auth;
using WebPage.Connections;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Backend.DTO.User.Auth;
using System.Text.Json;
using WpfApp.Session;

namespace WpfApp.Pages.User.Auth
{
    public partial class SignInWindow : Window
    {
        private LoginService? _loginService;
        private bool _passedFirstPart = false, _passedSecondPart = false, _passedThirdPart = false;
        private int currentStep = 1;
        public SignInWindow (LoginService loginService) {
            InitializeComponent ();
            UpdateButtonState ();
            _loginService = loginService; 
            TbUsername.TextChanged += (s, e) => UpdateButtonState ();
            TbPassword.TextChanged += (s, e) => UpdateButtonState ();
        }

        public SignInWindow()
        {
            InitializeComponent();
            UpdateButtonState();
            TbUsername.TextChanged += (s, e) => UpdateButtonState();
            TbPassword.TextChanged += (s, e) => UpdateButtonState();
        }

        private void UpdateUISteps () {
            Step1Panel.IsEnabled = currentStep == 1;
            Step2Panel.IsEnabled = currentStep == 2;
            Step3Panel.IsEnabled = currentStep == 3;
        }

        private void NavigateToMainWindow () {
            var mainWindow = new WindowContainer ();
            mainWindow.Show ();
            Close ();
        }

        private void UpdateButtonState () {
            BtnSignIn.IsEnabled = !string.IsNullOrWhiteSpace (TbUsername.Text) &&
                !string.IsNullOrWhiteSpace (TbPassword.Text);
        }
        private void TbPassword_TextChanged (object sender, TextChangedEventArgs e) {

        }

        private void PbPassword_PasswordChanged (object sender, RoutedEventArgs e) {
            if (sender is TextBox textBox && PbPassword.Password != textBox.Text)
                PbPassword.Password = textBox.Text;
            else if (sender is PasswordBox passwordBox && TbPassword.Text != passwordBox.Password)
                TbPassword.Text = passwordBox.Password;
        }

        private async void BtnSignIn_Click (object sender, RoutedEventArgs e)
        {
            BtnSignIn.IsEnabled = false;

            var loginDTO = new LoginDTO
            {
                username = TbUsername.Text,
                password = TbPassword.Text
            };

            try
            {
                string url = AuthEndpoints.ValidateCredentials;
                var response = await HttpClientHelper.PostAsync(url, loginDTO);

                if (response.IsSuccessStatusCode)
                {
                    Step1Panel.IsEnabled = false;
                    currentStep = 2;
                    UpdateUISteps();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection: {ex.Message}", AlertType.ERROR);
            }

            BtnSignIn.IsEnabled = true;
        }

        private async void BtnSendCode_Click (object sender, RoutedEventArgs e)
        {
            string username = TbUsername.Text;
            string email = TbEmail.Text.Trim();

            var payload = new { username, email };

            try
            {
                string url = AuthEndpoints.CreateTwoFactorCode;
                var response = await HttpClientHelper.PatchAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    Step2Panel.IsEnabled = false;
                    currentStep = 3;
                    UpdateUISteps();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection: {ex.Message}", AlertType.ERROR);
            }
        }
        private async void BtnValidateCode_Click(object sender, RoutedEventArgs e)
        {
            string username = TbUsername.Text.Trim();
            string code = TbVerificationCode.Text.Trim();

            var dto = new CodeTwoFactorDTO
            {
                username = username,
                twoFactorCode = code
            };

            try
            {
                string url = AuthEndpoints.ValidateTwoFactorCode;
                var response = await HttpClientHelper.PostAsync(url, dto);

                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<TwoFactorResponseDTO>(resultJson);

                    if (result != null)
                    {
                        UserSession.Instance.SetUser(username, result.jwtToken);
                        MessageBox.Show("Inicio de sesion exitoso");
                        NavigateToMainWindow();
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection: {ex.Message}", AlertType.ERROR);
            }
        }

        private void ChbShowPassword_Checked (object sender, RoutedEventArgs e)
        {
            PasswordUtilities.ShowPassword (TbPassword, PbPassword);
        }

        private void ChbShowPassword_Unchecked (object sender, RoutedEventArgs e)
        {
            PasswordUtilities.HidePassword (TbPassword, PbPassword);
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            var signUpWindow = new SignUpWindow();
            signUpWindow.Show();

            Application.Current.MainWindow = signUpWindow;
            this.Close();
        }
    }
}
