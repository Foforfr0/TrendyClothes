using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Security.Cryptography;

namespace WpfApp.Utilities
{
    public static class PasswordUtilities
    {
        public static void ShowPassword(TextBox TbPassword, PasswordBox PbPassword)
        {
            TbPassword.Text = PbPassword.Password;

            PbPassword.Visibility = Visibility.Collapsed;
            TbPassword.Visibility = Visibility.Visible;

            TbPassword.SelectionStart = TbPassword.Text.Length;
            TbPassword.SelectionLength = 0;
            TbPassword.Focus();
        }

        public static void HidePassword(TextBox TbPassword, PasswordBox PbPassword)
        {
            PbPassword.Password = TbPassword.Text;

            TbPassword.Visibility = Visibility.Collapsed;
            PbPassword.Visibility = Visibility.Visible;

            PbPassword.Focus();
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return null;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder(64);
                foreach (byte b in hash)
                    result.Append(b.ToString("x2"));

                return result.ToString();
            }
        }

        public static bool IsPasswordSecure(string password)
        {
            return Regex.IsMatch(password, Constants.SAFE_PASSWORD_PATTERN);
        }
    }
}
