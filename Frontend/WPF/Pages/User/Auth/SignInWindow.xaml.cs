using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Auth
{
    /// <summary>
    /// Lógica de interacción para SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        private int currentStep = 1;
        public SignInWindow()
        {
            InitializeComponent();
            UpdateButtonState();
            TbUsername.TextChanged += (s, e) => UpdateButtonState();
            TbPassword.TextChanged += (s, e) => UpdateButtonState();
        }

        private void UpdateUISteps()
        {
            Step1Panel.IsEnabled = currentStep == 1;
            Step2Panel.IsEnabled = currentStep == 2;
            Step3Panel.IsEnabled = currentStep == 3;
        }

        private void NavigateToMainWindow()
        {
            var mainWindow = new WindowContainer();
            mainWindow.Show();
            Close();
        }

        private void UpdateButtonState()
        {
            BtnSignIn.IsEnabled = !string.IsNullOrWhiteSpace(TbUsername.Text) &&
                !string.IsNullOrWhiteSpace(TbPassword.Text);
        }
        private void TbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && PbPassword.Password != textBox.Text)
                PbPassword.Password = textBox.Text;
            else if (sender is PasswordBox passwordBox && TbPassword.Text != passwordBox.Password)
                TbPassword.Text = passwordBox.Password;
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: login validation

            currentStep = 2;
            UpdateUISteps();
        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            currentStep = 3;
            UpdateUISteps();
        }

        private void BtnValidateCode_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainWindow();
        }

        private void ChbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordUtilities.ShowPassword(TbPassword, PbPassword);
        }

        private void ChbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordUtilities.HidePassword(TbPassword, PbPassword);
        }
    }
}
