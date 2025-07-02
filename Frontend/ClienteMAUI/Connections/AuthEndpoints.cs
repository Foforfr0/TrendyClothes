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

        public static string GetProducts => $"{BaseUrl}/Search";

    }
    public static class UserEndpoints
    {
        private const string BaseUrl = "http://10.0.2.2:5000/api/MyProducts";
        private const string BaseUrl2 = "http://10.0.2.2:5000/api/MyProduct";
        public static string GetMyProducts(string username) => $"{BaseUrl}/Search?username={Uri.EscapeDataString(username)}";
        public static string DeleteProduct(int idproduct) => $"{BaseUrl2}/Delete/{idproduct}";

    }
    public static class GRPCEndpoints
    {

    }

}
