using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WpfApp.Components
{
    public partial class Carousel : UserControl
    {
        private readonly List<UserControl> _cards = new();
        private int _currentIndex = 0;
        public Carousel()
        {
            InitializeComponent();
            DataContext = this;

            _cards.Add(new CategorieCard2());
            _cards.Add(new ItemCard2());
            _cards.Add(new CategorieCard2());

            ShowCurrentCard();
        }

        private void ShowCurrentCard()
        {
            if (_cards.Count == 0) return;
            CardHost.Content = _cards[_currentIndex];
        }
        private void BtnLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_cards.Count == 0) return;

            _currentIndex = (_currentIndex - 1 + _cards.Count) % _cards.Count;
            ShowCurrentCard();
        }

        private void BtnRight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_cards.Count == 0) return;

            _currentIndex = (_currentIndex + 1) % _cards.Count;
            ShowCurrentCard();
        }
    }
}
