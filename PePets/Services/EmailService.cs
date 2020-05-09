using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace PePets.Services
{
    public class EmailService
    {
        private readonly string companyName;
        private readonly string companyEmail;
        private readonly string companyEmailPassword;

        public EmailService()
        {
            companyName = "PePets.ru";
            companyEmail = "youoliver@mail.ru";
            companyEmailPassword = "1635148rus73";
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(companyName, companyEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru");
                await client.AuthenticateAsync(companyEmail, companyEmailPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
