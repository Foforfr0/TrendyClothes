using System.Text.Json;
using System.Text;

namespace WebPage.Auth {
    public class ManageJWT {
        private static string? GetClaimFromJwt (string jwt, string claimName) {
            try {
                string[]? parts = jwt.Split ('.');
                if (parts.Length != 3)
                    return null;

                string? payload = parts[1];
                // Padding si es necesario (base64url sin padding puede fallar)
                payload = payload.PadRight (payload.Length + (4 - payload.Length % 4) % 4, '=');
                byte[] bytes = Convert.FromBase64String (payload.Replace ('-', '+').Replace ('_', '/'));

                string? json = Encoding.UTF8.GetString (bytes);
                Dictionary<string, object>? payloadData = JsonSerializer.Deserialize<Dictionary<string, object>> (json);

                if (payloadData != null && payloadData.ContainsKey (claimName))
                    return payloadData[claimName]?.ToString ();

                return null;
            } catch {
                return null;
            }
        }

        public static string? GetNameJWT (string jwt) => GetClaimFromJwt (jwt, "Name");
        public static string? GetRoleJWT (string jwt) => GetClaimFromJwt (jwt, "role");
        public static string? GetSidJWT (string jwt) => GetClaimFromJwt (jwt, "sid");

    }
}
