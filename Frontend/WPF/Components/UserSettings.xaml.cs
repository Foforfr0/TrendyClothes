using System.Windows;
using System.Windows.Controls;
using WpfApp.Utilities;
using WpfApp.Pages;
using WpfApp.Pages.User.Profile;

namespace WpfApp.Components
{
    public partial class UserSettings : UserControl
    {
        public UserSettings()
        {
            InitializeComponent();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoToProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToPage("Glb_Profile", new UserProfilePage());
        }
    }
}
