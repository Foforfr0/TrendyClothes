using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WpfApp.Utilities
{
    public class InputUtilities
    {
        public static void ValidateInput(TextBox textBox, string pattern, int maxLength)
        {
            textBox.TextChanged += (s, e) =>
            {
                string input = textBox.Text;
                string cleaned = Regex.Replace(input, pattern, "");

                if (cleaned.Length > maxLength)
                    cleaned = cleaned.Substring(0, maxLength);

                if (input != cleaned)
                {
                    textBox.Text = cleaned;
                    textBox.SelectionStart = cleaned.Length;
                    Animations.ShakeTextBox(textBox);
                }
            };
        }

        public static void ValidatePriceInput(TextBox textBox, string pattern = @"^\d{0,3}(\.\d{0,2})?$", decimal max_monetary_Value = 999.99m)
        {
            textBox.PreviewTextInput += (sender, e) =>
            {
                string currentText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

                if (!Regex.IsMatch(currentText, pattern))
                {
                    e.Handled = true;
                    Animations.ShakeTextBox(textBox);
                    return;
                }

                if (decimal.TryParse(currentText, out decimal value) && value > max_monetary_Value)
                {
                    e.Handled = true;
                    Animations.ShakeTextBox(textBox);
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                string rawText = textBox.Text.Replace(",", "").Replace("$", "").Trim();

                if (string.IsNullOrEmpty(rawText) || decimal.TryParse(rawText, out decimal val) && val == 0)
                {
                    textBox.Clear();
                    return;
                }

                if (decimal.TryParse(rawText, out decimal value))
                {
                    if (value > max_monetary_Value)
                        value = max_monetary_Value;

                    textBox.Text = string.Format(CultureInfo.InvariantCulture, "${0:N2}", value);
                }
                else textBox.Text = string.Empty;
            };

            textBox.GotFocus += (sender, e) =>
            {
                string text = textBox.Text.Replace(",", "").Replace("$", "").Trim();

                if (string.IsNullOrEmpty(text))
                {
                    textBox.Text = string.Empty;
                    return;
                }

                if (decimal.TryParse(text, out decimal value))
                {
                    bool hasDecimals = value % 1 != 0;

                    textBox.Text = hasDecimals
                        ? value.ToString("0.00", CultureInfo.InvariantCulture)
                        : value.ToString("0", CultureInfo.InvariantCulture);
                }
                else
                {
                    textBox.Text = string.Empty;
                }

                textBox.SelectionStart = textBox.Text.Length;
            };
        }

        public static void ValidatePasswordInput(PasswordBox passwordBox, string pattern, int maxLength)
        {
            passwordBox.PasswordChanged += (s, e) =>
            {
                string input = passwordBox.Password;
                string cleaned = Regex.Replace(input, pattern, "");

                if (cleaned.Length > maxLength)
                    cleaned = cleaned.Substring(0, maxLength);

                if (input != cleaned)
                {
                    passwordBox.Password = cleaned;
                    Animations.ShakePasswordBox(passwordBox);
                }
            };
        }

        public static void ConvertToUpperCase(TextBox textBox)
        {
            textBox.TextChanged += (s, e) =>
            {
                textBox.Text = textBox.Text.ToUpper();
                textBox.SelectionStart = textBox.Text.Length;
            };
        }

        public static void ConvertToLowerCase(TextBox textBox)
        {
            textBox.TextChanged += (s, e) =>
            {
                textBox.Text = textBox.Text.ToLower();
                textBox.SelectionStart = textBox.Text.Length;
            };
        }

        public static bool IsValidEmailFormat(string email)
        {
            return Regex.IsMatch(email, Constants.EMAIL_FORMAT_PATTERN);
        }


        public static void ValidateDecimalInput(TextBox textBox)
        {
            string pattern = Constants.MONETARY_DECIMAL_PATTERN;

            textBox.PreviewTextInput += (sender, e) =>
            {
                string currentText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
                if (!Regex.IsMatch(currentText, pattern))
                {
                    e.Handled = true;
                    Animations.ShakeTextBox(textBox);
                }
            };

        }
    }
}
