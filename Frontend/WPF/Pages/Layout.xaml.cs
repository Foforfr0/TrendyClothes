using System.Windows.Controls;
using WpfApp.Components;

namespace WpfApp.Pages {
    /// <summary>
    /// Interaction logic for Layout.xaml
    /// </summary>
    public partial class Layout : Page {
        public Layout () {
            InitializeComponent ();

            Navbar.SetFrame (FrameContainerLayout);
            FrameContainerLayout.Navigate (new Index ());
        }
    }
}
