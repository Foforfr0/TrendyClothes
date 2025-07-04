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
        private const string BaseUrl2 = "http://localhost:5000/api/Product";
        private const string BaseUrl3 = "http://localhost:5000/api/MyProduct";


        //register products
        public static string GetCategories => $"{BaseUrl2}/Tags/Categories";
        public static string GetTypes => $"{BaseUrl2}/Tags/Types";
        public static string GetStatuses => $"{BaseUrl2}/Tags/Statusses";
        public static string GetProducts => $"{BaseUrl2}/Search";
        public static string GetProductDetails(int id) => $"{BaseUrl2}/Details?id={id}";
        public static string DeleteProduct(int idproduct) => $"{BaseUrl3}/Delete?id={idproduct}";

        public static string CreateProduct => $"{BaseUrl2}";
        public static string UpdateProduct => $"{BaseUrl2}/Edit";

        //user products
        public static string GetMyProducts(string username) => $"{BaseUrl}/Search?username={Uri.EscapeDataString(username)}";
    }
}


