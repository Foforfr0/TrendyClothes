using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Session
{
    public class UserSession
    {
        private static UserSession? _instance;

        public static UserSession Instance => _instance ??= new UserSession();

        // Datos de sesión
        public string? Username { get; private set; }
        public string? JwtToken { get; private set; }

        private UserSession() { }

        public void SetUser(string username, string jwtToken)
        {
            Username = username;
            JwtToken = jwtToken;
        }

        public void Clear()
        {
            Username = null;
            JwtToken = null;
        }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(JwtToken);
    }
}
