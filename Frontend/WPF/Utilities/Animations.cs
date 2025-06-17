using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace WpfApp.Utilities
{
    public class Animations
    {
        public static void BeginAnimation(UIElement targetElement, string resourceKey, Action onComplete = null)
        {
            if (targetElement == null || string.IsNullOrEmpty(resourceKey)) return;

            var animation = Application.Current.Resources[resourceKey] as Storyboard;
            if (animation != null)
            {
                var clonedAnimation = animation.Clone();

                if (onComplete != null)
                    clonedAnimation.Completed += (s, e) => onComplete();

                if (targetElement is FrameworkElement frameworkElement)
                    clonedAnimation.Begin(frameworkElement);
            }
        }

        public static void ShakeTextBox(TextBox textBox)
        {
            if (textBox == null) return;

            var mainGrid = (Grid)textBox.Template.FindName("MainGrid", textBox);

            if (mainGrid != null)
            {
                var storyboard = Application.Current.Resources["ShakeAnimation"] as Storyboard;
                if (storyboard != null)
                {
                    storyboard.Begin(mainGrid, true);
                    return;
                }
            }

            if (textBox.RenderTransform == null || !(textBox.RenderTransform is TranslateTransform))
                textBox.RenderTransform = new TranslateTransform();

            BeginAnimation(textBox, "ShakeAnimation");
        }


        public static void ShakePasswordBox(PasswordBox passwordBox)
        {
            if (passwordBox == null) return;

            if (passwordBox.RenderTransform == null || !(passwordBox.RenderTransform is TranslateTransform))
                passwordBox.RenderTransform = new TranslateTransform();

            BeginAnimation(passwordBox, "ShakeAnimation");
        }
    }
}
