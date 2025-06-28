using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
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
                (TbLastName, Utilities.Constants.NAMES_PATTERN, Utilities.Constants.MAX_LENGTH_NAMES),
                (TbAccountPassword, Utilities.Constants.ALPHANUMERIC_PATTERN, Utilities.Constants.MAX_LENGTH_PASSWORD),
                (TbEmailAddress, Constants.EMAIL_ALLOWED_CHARS_PATTERN, Constants.MAX_LENGTH_EMAIL)
            };

            foreach (var (textBox, pattern, maxLength) in validations)
                InputUtilities.ValidateInput(textBox, pattern, maxLength);

            InputUtilities.ValidatePasswordInput(PbAccountPassword, Utilities.Constants.ALPHANUMERIC_PATTERN,
                Utilities.Constants.MAX_LENGTH_PASSWORD);
            InputUtilities.ConvertToLowerCase(TbEmailAddress);
        }

        private void UpdateFormButtonState(Button button)
        {
            var requiredFields = new List<TextBox>
            {
                TbFirstName, TbLastName, TbEmailAddress, TbAccountPassword, TbConfirmPassword
            };

            bool allFieldsFilled = true;

            foreach (var field in requiredFields)
            {
                if (string.IsNullOrWhiteSpace(field.Text))
                {
                    allFieldsFilled = false;
                    break;
                }
            }

            button.IsEnabled = allFieldsFilled;
        }

        private void UpdateRegisterButtonState()
        {
            var requiredFields = new List<object>
            {
                TbFirstName,
                TbLastName,
                TbEmailAddress,
                TbAccountPassword,
                TbConfirmPassword
            };

            bool allFieldsFilled = true;
            foreach (TextBox field in requiredFields)
            {
                if (string.IsNullOrWhiteSpace(field.Text))
                {
                    allFieldsFilled = false;
                    break;
                }
            }

            BtnRegisterUser.IsEnabled = allFieldsFilled;
        }

        private void SelectProfilePicture(Image targetImageControl)
        {
            var dialogTitle = Application.Current.Resources["RegUser_DialogSelectProfilePic"]?.ToString();
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = Utilities.Constants.IMAGE_FILE_FILTER,
                Title = dialogTitle
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var processedImageBytes = ImageUtilities.ProcessImageBeforeSaving(openFileDialog.FileName);

                    if (!ImageUtilities.IsImageSizeValid(processedImageBytes))
                    {
                        MessageDialog.Show("GlbDialogT_InvalidImageSize", "GlbDialogD_InvalidImageSize", AlertType.WARNING);
                        return;
                    }

                    UserProfilePic.Source = ImageUtilities.ConvertToImageSource(processedImageBytes);
                    BtnDeleteImage.IsEnabled = true;
                }
                catch
                {
                    MessageDialog.Show("GlbDialogT_InvalidImageSize", "GlbDialogD_InvalidImageSize", AlertType.WARNING);
                }
            }
        }

        private void RequiredFields_TextChanged(object sender, RoutedEventArgs e)
        {
            UpdateRegisterButtonState();
        }

        private void Password_TextChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && PbAccountPassword.Password != textBox.Text)
                PbAccountPassword.Password = textBox.Text;
            else if (sender is PasswordBox passwordBox && TbAccountPassword.Text != passwordBox.Password)
                TbAccountPassword.Text = passwordBox.Password;
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            SelectProfilePicture(UserProfilePic);
        }

        private void BtnDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            BtnDeleteImage.IsEnabled = false;
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

        private void BtnRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog.Show("RegUser_DialogTSuccess", "RegUser_DialogDSuccess", AlertType.SUCCESS);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var container = new WindowContainer();
            container.Show();
            this.Close();
        }

    }
}
