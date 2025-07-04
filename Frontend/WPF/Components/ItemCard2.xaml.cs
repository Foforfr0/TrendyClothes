using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Pages.Product;
using WpfApp.Utilities;

namespace WpfApp.Components {
    public partial class ItemCard2 : UserControl {
        public bool isSelectable { get; set; } = false;
        public event EventHandler CardSelected;
        public ItemCard2 () {
            InitializeComponent ();
        }

        private void Border_MouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            if (isSelectable) {
                CardSelected?.Invoke (this, EventArgs.Empty);
            } else {
                NavigationManager.Instance.NavigateToPage ("Product_Header", new ProductDetailsPage ());
            }
        }
    }
}
