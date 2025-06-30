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
using WpfApp.Pages.Dialogs;
using WpfApp.Utilities;

namespace WpfApp.Pages.Product
{
    public partial class ProductDetailsPage : Page
    {
        private Button _selectedSizeButton;
        public ProductDetailsPage()
        {
            InitializeComponent();
            UpdateQuantityDisplay();
        }

        private void UpdateQuantityDisplay()
        {
            //TODO
        }

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog.Show("Glb_AddedItemT", "Glb_AddedItemD", AlertType.SUCCESS);
        }

        private void BtnBuyNow_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            UpdateQuantityDisplay();
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            UpdateQuantityDisplay();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }

        private void BtnSize_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton == null) return;

            if (_selectedSizeButton != null)
                _selectedSizeButton.Style = (Style)FindResource("SecondaryButtonNoIconStyle");

            clickedButton.Style = (Style)FindResource("PrimaryButtonNoIconStyle");

            _selectedSizeButton = clickedButton;
        }
    }
}
