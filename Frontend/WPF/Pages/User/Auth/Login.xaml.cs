using System.Net;
using System.Windows;
using System.Windows.Controls;
using WpfApp.DTO;
using WpfApp.Services.User.Auth;

namespace WpfApp.Pages.User.Auth {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page {
        private LoginService? _loginService;
        private bool _passedFirstPart = false, _passedSecondPart = false, _passedThirdPart = false;

        public Login () {
            InitializeComponent ();
            _loginService = new LoginService ();
        }

        private async void ClicFirstValidation (object sender, RoutedEventArgs e) {
            string username = textBox_Username.Text;
            string password = textBox_Password.Text;

            _loginService = new LoginService ();
            MessageResponse<HttpStatusCode> response = await _loginService.LoginAsync (username, password);
            _loginService = null;

            if (response.dataRetrieved == HttpStatusCode.OK)
                _passedFirstPart = true;

            MessageBox.Show (response.message);
        }

        private async void ClicSecondValidation (object sender, RoutedEventArgs e) {
            string username = textBox_Username.Text;
            string email = textBox_Email.Text;

            _loginService = new LoginService ();
            MessageResponse<HttpStatusCode> response = await _loginService.ValidateEmailUserAsync (username, email);
            _loginService = null;
            if (response.dataRetrieved == HttpStatusCode.OK) {
                _loginService = new LoginService ();
                MessageResponse<HttpStatusCode> response2 = await _loginService.CreateTwoFactorCodeAsync (username, email);
                _loginService = null;
                if (response2.dataRetrieved == HttpStatusCode.OK) {
                    MessageBox.Show (response2.message);
                    _passedSecondPart = true;
                } else
                    MessageBox.Show (response.message);
            } else {
                MessageBox.Show (response.message);
            }
        }

        private async void ClicThirdValidation (object sender, RoutedEventArgs e) {
            string username = textBox_Username.Text;
            string twoFactorCode = textBox_TwoFactorCode.Text;

            _loginService = new LoginService ();
            MessageResponse<HttpStatusCode> response = await _loginService.ValidateTwoFactorCodeAsync (username, twoFactorCode);
            _loginService = null;
            if (response.dataRetrieved == HttpStatusCode.OK) {
                _loginService = new LoginService ();
                await _loginService.DeleteTwoFactorCodeAsync (username);
                _loginService = null;
                _passedThirdPart = true;
            }
            MessageBox.Show (response.message);

            if (_passedFirstPart && _passedSecondPart && _passedThirdPart)
                WindowContainer.SetFrame (new Layout ());
        }
    }
}
