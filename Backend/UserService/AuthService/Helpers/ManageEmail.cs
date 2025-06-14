using MailKit.Net.Smtp;
using MimeKit;

namespace AuthService.Helpers {
    public class ManageEmail {
        /* Injection dependencies
                 "EmailSettings": {
            "FromName": "TrendyClothes",
            "FromEmail": "trendyclothesa129@gmail.com",
            "SmtpHost": "smtp.gmail.com",
            "SmtpPort": 587,
            "Username": "trendyclothesa129@gmail.com",
            "Password": "!-:v3TYtALce8Rr"
         */
        private string _fromName = "TrendyClothes";
        private string _fromEmail = "foforfr007@gmail.com";
        private string _smtpHost = "smtp.gmail.com";
        private int _smtpPort = 587;
        private string _username = "foforfr007@gmail.com";
        private string _password = "kbxn bjnp qftg gime";

        public async Task SendAsync (string username, string toEmail, string twoFactorCode) {
            try {
                MimeMessage? message = new MimeMessage ();
                message.From.Add (new MailboxAddress (_fromName, _fromEmail));
                message.To.Add (new MailboxAddress (username, toEmail));
                message.Subject = "Código de verificación de inicio de sesión - TrendyClothes";
                message.Body = new TextPart ("html") {
                    Text = $@"
                    <p>Hola {username},</p>
                    <p>Tu código de verificación es:</p>
                    <h2 style='color:#2e6c80;'>{twoFactorCode}</h2>
                    <p>No compartas este código con nadie.</p>
                    <br>
                    <small>Este mensaje fue generado automáticamente.</small>"
                };
                using SmtpClient? client = new SmtpClient ();
                await client.ConnectAsync (_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync (_username, _password);
                await client.SendAsync (message);
                await client.DisconnectAsync (true);
            } catch (Exception ex) {
                throw new InvalidOperationException ("No se pudo enviar el correo electrónico.", ex);
            }
        }
    }
}