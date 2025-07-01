using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Pages;
using WpfApp.Pages.Dialogs;
using WpfApp.Pages.User.Auth;
using WpfApp.Pages.User.Profile;
using WpfApp.Services.User.Auth;
using WpfApp.Utilities;

namespace WpfApp.Components
{
    public partial class UserSettings : UserControl
    {
        public UserSettings()
        {
            InitializeComponent();
            //LoadUsername();
        }

        public static void Show(FrameworkElement triggerButton)
        {
            var userSettings = new UserSettings();
            var windowContainer = Application.Current.MainWindow as WindowContainer;

            if (windowContainer == null || triggerButton == null)
                return;

            windowContainer.PopUpHost.Content = userSettings;

            userSettings.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size popupSize = userSettings.DesiredSize;

            Point relativePoint = triggerButton.TransformToAncestor(windowContainer)
                .Transform(new Point(0, 0));

            double margin = 8;

            double left = relativePoint.X;
            double top = relativePoint.Y + triggerButton.ActualHeight + margin;

            double windowWidth = windowContainer.ActualWidth;
            double popupRight = left + popupSize.Width;

            if (popupRight > windowWidth)
            {
                left = windowWidth - popupSize.Width - margin;

                if (left < margin)
                    left = margin;
            }

            Canvas.SetLeft(windowContainer.PopUpHost, left);
            Canvas.SetTop(windowContainer.PopUpHost, top);

            windowContainer.PopUpOverlay.Visibility = Visibility.Visible;

            windowContainer.PopUpOverlay.MouseLeftButtonDown -= PopUpOverlay_MouseLeftButtonDown;
            windowContainer.PopUpOverlay.MouseLeftButtonDown += PopUpOverlay_MouseLeftButtonDown;
        }



        private static void PopUpOverlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var windowContainer = Application.Current.MainWindow as WindowContainer;
            windowContainer.PopUpOverlay.Visibility = Visibility.Collapsed;
            windowContainer.PopUpHost.Content = null;

            windowContainer.PopUpOverlay.MouseLeftButtonDown -= PopUpOverlay_MouseLeftButtonDown;
        }

        private void NavigateToSignIn()
        {
            NavigationManager.Reset();
            var loginService = App.Services?.GetRequiredService<LoginService> ();
            var signIn = new SignInWindow(loginService);
            signIn.Show();

            Application.Current.MainWindow.Close();
        }

        private void NavigateToSignUp()
        {
            NavigationManager.Reset();
            var signUp = new SignUpWindow();
            signUp.Show();

            Application.Current.MainWindow.Close();
        }

        private void LogOut()
        {
            //TODO not a void
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            //MessageDialog.ShowConfirm();
        }

        private void GoToProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToPage("Glb_Profile", new UserProfilePage());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSignIn();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSignUp();
        }
    }
}
