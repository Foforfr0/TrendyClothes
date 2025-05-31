using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp.Components {
    /// <summary>
    /// Interaction logic for ProductCard.xaml
    /// </summary>
    public partial class CategorieCard : UserControl {
        public CategorieCard () {
            InitializeComponent ();
        }

        public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register (
            nameof (ImageSource), typeof (ImageSource), typeof (CategorieCard));

        public ImageSource ImageSource {
            get => (ImageSource)GetValue (ImageSourceProperty);
            set => SetValue (ImageSourceProperty, value);
        }

        public static readonly DependencyProperty CategoryNameProperty =
            DependencyProperty.Register (
                nameof (CategoryName), typeof (string), typeof (CategorieCard));

        public string CategoryName {
            get => (string)GetValue (CategoryNameProperty);
            set => SetValue (CategoryNameProperty, value);
        }

        // Evento para que el contenedor escuche cuando se hace clic
        public event RoutedEventHandler? ShowCategoryClicked;

        private void ClickShowCategory (object sender, RoutedEventArgs e) {
            ShowCategoryClicked?.Invoke (this, e);
        }
    }
}
