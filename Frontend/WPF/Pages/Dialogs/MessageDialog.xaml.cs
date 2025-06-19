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

namespace WpfApp.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : UserControl
    {
        private Action _onConfirm;
        public static Action OnCancel;
        public static readonly (string Brush, string ButtonText) DangerStyle = ("DangerBrush", "Glb_Delete");

        public MessageDialog()
        {
            InitializeComponent();
        }

        public static void Show(string title, string description, AlertType alertType, Action onConfirm = null)
        {
            var activeWindow = GetActiveWindow();
            if (activeWindow == null) return;

            var dialog = new MessageDialog
            {
                _onConfirm = onConfirm
            };

            dialog.Opacity = 0;
            dialog.DialogTitle.Text = FindResourceString(title);
            dialog.DialogDescription.Text = FindResourceString(description);
            dialog.ConfigureAlertType(alertType);

            Action showAction = () =>
            {
                var dialogHost = activeWindow.FindName("DialogHost") as ContentControl;
                var dialogOverlay = activeWindow.FindName("DialogOverlay") as Border;

                if (dialogHost == null || dialogOverlay == null) return;

                dialogHost.Content = dialog;
                dialogOverlay.Visibility = Visibility.Visible;
                Animations.BeginAnimation(dialog, "PopupFadeInAnimation");
            };

            if (Application.Current.Dispatcher.CheckAccess())
                showAction();
            else
                Application.Current.Dispatcher.Invoke(showAction);
        }

        public static void ShowConfirm(string title, string description, Action onConfirm,
            string dangerButtonTextResourceKey = null)
        {
            var activeWindow = GetActiveWindow();
            if (activeWindow == null) return;

            var dialog = new MessageDialog
            {
                _onConfirm = onConfirm
            };

            dialog.DialogTitle.Text = FindResourceString(title);
            dialog.DialogDescription.Text = FindResourceString(description);
            dialog.SetAlertProperties("PrimaryButtonHoverBrush", "confirm-alert-icon.png", "Glb_Accept", Visibility.Visible);
            dialog.BtnClose.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(dangerButtonTextResourceKey))
                dialog.BtnAccept.Content = FindResourceString("Glb_Confirm");
            else
            {
                dialog.BtnAccept.Background = (SolidColorBrush)Application.Current.Resources["DangerBrush"];
                dialog.BtnAccept.Content = FindResourceString(dangerButtonTextResourceKey);
            }

            Action showAction = () =>
            {
                var dialogHost = activeWindow.FindName("DialogHost") as ContentControl;
                var dialogOverlay = activeWindow.FindName("DialogOverlay") as Border;

                if (dialogHost == null || dialogOverlay == null) return;

                dialogHost.Content = dialog;
                dialogOverlay.Visibility = Visibility.Visible;
                Animations.BeginAnimation(dialog, "PopupFadeInAnimation");
            };

            if (Application.Current.Dispatcher.CheckAccess())
                showAction();
            else
                Application.Current.Dispatcher.Invoke(showAction);
        }

        private static string FindResourceString(string resourceKey)
        {
            var resource = Application.Current.TryFindResource(resourceKey);
            return resource != null ? resource.ToString() : $"{resourceKey}";
        }

        private void ConfigureAlertType(AlertType alertType)
        {
            switch (alertType)
            {
                case AlertType.SUCCESS:
                    SetAlertProperties("SuccessBrush", "success-alert-icon.png", "Glb_Accept", Visibility.Collapsed);
                    break;
                case AlertType.WARNING:
                    SetAlertProperties("WarningBrush", "warning-alert-icon.png", "Glb_TryAgain", Visibility.Collapsed);
                    break;
                case AlertType.ERROR:
                    SetAlertProperties("DangerBrush", "error-alert-icon.png", "Glb_Close", Visibility.Collapsed);
                    break;
            }
        }

        private void SetAlertProperties(string brushResourceKey, string iconSource, string buttonTextResourceKey, Visibility secondaryButtonVisibility)
        {
            var brush = (SolidColorBrush)Application.Current.Resources[brushResourceKey];

            HeaderBorder.Background = brush;
            AlertIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri($"/Resources/Icons/{iconSource}", UriKind.Relative));
            BtnAccept.Content = FindResourceString(buttonTextResourceKey);
            BtnClose.Visibility = secondaryButtonVisibility;
        }

        private void Click_BtnAccept(object sender, RoutedEventArgs e)
        {
            _onConfirm?.Invoke();
            CloseDialog();
        }

        private void Click_BtnCancel(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
            OnCancel = null;
            CloseDialog();
        }

        private void CloseDialog()
        {
            var activeWindow = GetActiveWindow();
            if (activeWindow != null)
            {
                var dialogHost = activeWindow.FindName("DialogHost") as ContentControl;
                var dialogOverlay = activeWindow.FindName("DialogOverlay") as Border;

                if (dialogHost != null && dialogOverlay != null)
                {
                    dialogHost.Content = null;
                    dialogOverlay.Visibility = Visibility.Collapsed;
                }
            }
        }

        private static Window GetActiveWindow()
        {
            return Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive);
        }
    }

    public enum AlertType
    {
        SUCCESS,
        WARNING,
        ERROR
    }
}
