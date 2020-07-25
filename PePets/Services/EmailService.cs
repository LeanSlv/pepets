using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace PePets.Services
{
    public class EmailService
    {
        private readonly string name;
        private readonly string login;
        private readonly string password;
        private readonly string subject;

        public EmailService(string emailLogin, string emailPassword)
        {
            name = "PePets.ru";
            login = emailLogin;
            password = emailPassword;
            subject = "Подтвердите ваш email aдрес для регистрации на сайте PePets.ru";
        }

        public async Task SendEmailAsync(string userName, string toEmail, string callbackUrl)
        {
            var emailMessage = new MimeMessage();

            string message = GenerateMessageBody(userName, callbackUrl);

            emailMessage.From.Add(new MailboxAddress(name, login));
            emailMessage.To.Add(new MailboxAddress(userName, toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru");
                await client.AuthenticateAsync(login, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        private string GenerateMessageBody(string userName, string callbackUrl)
        {
            return
                "<div>" +
                    "<div style = 'padding: 0.5em; margin: 0 auto; width: 50%; font-size: 1.2rem; font-family: Arial, Helvetica, sans-serif;'>" +
                        $"<div style = 'margin-bottom: 0.7em;' > Здравствуйте {userName},</div>" +
                        "<div style = 'margin-bottom: 1.5em;' > Для того, чтобы продолжить регистрацию на сайте PePets.ru, пожалуйста, подтвердите ваш email адрес:</div>" +
                        $"<a style = 'display: flex; padding: 1.5em; background-color: #587fcc; color: white; justify-content: center; text-decoration: none; border-radius: 25px;' href = '{callbackUrl}' > Подтверить email адрес</a>" +
                     "</div>" +
                "</div>";
        }
    }
}
