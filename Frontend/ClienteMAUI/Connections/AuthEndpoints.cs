using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMAUI.Connections
{
    public static class AuthEndpoints
    {
        private const string BaseUrl = "http://10.0.2.2:5000/api/User";

        public static string ValidateCredentials => $"{BaseUrl}/Login";
        public static string ValidateEmail(string username, string email) => $"{BaseUrl}/Login/ValidateEmail?username={Uri.EscapeDataString(username)}&email={Uri.EscapeDataString(email)}";
        public static string CreateTwoFactorCode => $"{BaseUrl}/Login/CreateTwoFactorCode";
        public static string ValidateTwoFactorCode => $"{BaseUrl}/Login/ValidateTwoFactorCode";
        public static string RegisterUser => $"{BaseUrl}/Registration";
    }
    public static class ProductEndpoints
    {
        private const string BaseUrl = "http://10.0.2.2:5000/api/Product";

        public static string GetCategories => $"{BaseUrl}/Tags/Categories";

    }
    public static class GRPCEndpoints
    {

    }

}
