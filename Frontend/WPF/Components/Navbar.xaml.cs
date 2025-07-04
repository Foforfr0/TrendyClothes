using System.Windows;
using System.Windows.Controls;
using WpfApp.Pages.Product;

namespace WpfApp.Components {
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl {

        private static Frame? _targetFrame;
        public event Action<string>? SearchSubmitted;

        public Navbar () {
            InitializeComponent ();
        }

        public static void SetFrame (Frame frame) {
            _targetFrame = frame;
        }

        private void ClickStart (object sender, RoutedEventArgs e) {
            _targetFrame?.Navigate (new ViewDetails());
        }

        private void ClickShowCategories (object sender, RoutedEventArgs e) {
            _targetFrame?.Navigate (new EditProduct());
        }

        private void ClickShowOffers (object sender, RoutedEventArgs e) {

        }

        private void ClickShowFashion (object sender, RoutedEventArgs e) {

        }

        private void ClickSearchProducts (object sender, RoutedEventArgs e)
        {
            string keyword = textBox_KeyWord.Text.Trim();

            if (!string.IsNullOrEmpty(keyword))
                SearchSubmitted?.Invoke(keyword);
        }

        private void ClickCheckAccount (object sender, RoutedEventArgs e) {

        }
    }
}
