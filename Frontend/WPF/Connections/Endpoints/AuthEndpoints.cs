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
}


