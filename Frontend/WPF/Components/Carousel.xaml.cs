using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WpfApp.Components
{
    /// <summary>
    /// Lógica de interacción para Carousel.xaml
    /// </summary>
    public partial class Carousel : UserControl
    {
        public Carousel()
        {
            InitializeComponent();
            DataContext = this;

            //TODO: Add ItemCards to the carousel
        }
    }
}
