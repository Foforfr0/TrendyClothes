using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.Pages.Dialogs;
using WpfApp.DTO;
using WpfApp.Pages.Dialogs;
using WpfApp.Services.User.Auth;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Auth {
    /// <summary>
    /// Lógica de interacción para SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window {
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

        private async void BtnSignIn_Click (object sender, RoutedEventArgs e) {
            string username = TbUsername.Text;
            string password = PbPassword.Password;

            MessageResponse<HttpStatusCode> response = await _loginService.LoginAsync (username, password);
            _loginService = null;

            if (response.dataRetrieved == HttpStatusCode.OK) {
                Step1Panel.IsEnabled = false;
                currentStep = 2;
                UpdateUISteps ();
                _passedFirstPart = true;
            } else {
                MessageBox.Show (response.message);
            }
        }

        private async void BtnSendCode_Click (object sender, RoutedEventArgs e) {
            string username = TbUsername.Text;
            string email = TbEmail.Text;

            MessageResponse<HttpStatusCode> response = await _loginService.ValidateEmailUserAsync (username, email);
            _loginService = null;
            if (response.dataRetrieved == HttpStatusCode.OK) {
                MessageResponse<HttpStatusCode> response2 = await _loginService.CreateTwoFactorCodeAsync (username, email);
                _loginService = null;
                if (response2.dataRetrieved == HttpStatusCode.OK) {
                    MessageBox.Show (response2.message);
                    _passedSecondPart = true;
                    Step2Panel.IsEnabled = false;
                    currentStep = 3;
                    UpdateUISteps ();
                } else
                    MessageBox.Show (response.message);
            } else {
                MessageBox.Show (response.message);
            }
        }
        private void BtnValidateCode_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog.Show("Login_DialogTSignedIn", "Login_DialogDSignedIn", AlertType.SUCCESS);
            NavigateToMainWindow();
        }

        private void ChbShowPassword_Checked (object sender, RoutedEventArgs e) {
            PasswordUtilities.ShowPassword (TbPassword, PbPassword);
        }

        private void ChbShowPassword_Unchecked (object sender, RoutedEventArgs e) {
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
