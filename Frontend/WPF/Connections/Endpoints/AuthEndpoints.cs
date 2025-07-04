namespace WebPage.Connections
{
    public static class AuthEndpoints
    {
        private const string BaseUrl = "http://localhost:5000/api/User";

        public static string ValidateCredentials => $"{BaseUrl}/Login";
        public static string ValidateEmail(string username, string email) =>
            $"{BaseUrl}/Login/ValidateEmail?username={Uri.EscapeDataString(username)}&email={Uri.EscapeDataString(email)}";

        public static string CreateTwoFactorCode => $"{BaseUrl}/Login/CreateTwoFactorCode";
        public static string ValidateTwoFactorCode => $"{BaseUrl}/Login/ValidateTwoFactorCode";
        public static string RegisterUser => $"{BaseUrl}/Registration";
    }

    public static class ProfileEndpoints
    {
        private const string BaseUrl = "http://localhost:5000/api/User";

        public static string GetPersonalData(string username) => $"{BaseUrl}/ViewProfile/GetPersonalData?username={username}";
    }

    public static class ProductEndpoints
    {
        private const string BaseUrl = "http://localhost:5000/api/MyProducts";

        public static string GetMyProducts(string username) => $"{BaseUrl}/Search?username={Uri.EscapeDataString(username)}";
    }
}


