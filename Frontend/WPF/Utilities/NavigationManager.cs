using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Pages;

namespace WpfApp.Utilities
{
    public class NavigationManager
    {
        private static NavigationManager _instance;

        private Frame mainFrame;

        public static NavigationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("No se ha inicializado el NavigationManager");
                }
                return _instance;
            }
        }

        private NavigationManager(Frame frame)
        {
            mainFrame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public static void Initialize(Frame frame)
        {
            if (_instance == null)
            {
                _instance = new NavigationManager(frame);
            }
        }

        public static void Reset()
        {
            _instance = null;
        }

        public class NavigationItem
        {
            public string Name { get; set; }
            public Page PageInstance { get; set; }
            public NavigationItem(string name, Page pageInstance)
            {
                Name = name;
                PageInstance = pageInstance;
            }
        }

        public void NavigateToPage(string resourceKey, Page pageInstance)
        {
            if (pageInstance == null)
                throw new ArgumentNullException(nameof(pageInstance));

            mainFrame.Navigate(pageInstance);
        }

        public void GoBack()
        {
            if (mainFrame?.CanGoBack == true)
                mainFrame.GoBack();
        }

        public void GoForward()
        {
            if (mainFrame?.CanGoForward == true)
                mainFrame.GoForward();
        }

    }
}
