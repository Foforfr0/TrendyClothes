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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Profile
{
    public partial class EditUserProfile : Page
    {
        public EditUserProfile()
        {
            InitializeComponent();
        }

        private void BtnModifyImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequiredFields_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ActualPassword_TextChanged(object sender, RoutedEventArgs e)
        {

        }
        private void ChbShowPassword_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ChbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void NewPassword_TextChanged(object sender, RoutedEventArgs e)
        {

        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }
    }
}
