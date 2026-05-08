using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration; // Ensure this is present

namespace ManchesterUnitedApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // FIX 2: Corrected all configuration keys to use "EmailSettings" (plural 's')
            var from = _configuration["EmailSettings:From"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];

            // FIX 2 & Safety: Using GetValue<int> for safer parsing of the Port
            var port = _configuration.GetValue<int>("EmailSettings:Port");

            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            // Check if essential configuration is missing before proceeding
            if (string.IsNullOrEmpty(smtpServer) || port == 0)
            {
                throw new InvalidOperationException("Email settings (SmtpServer or Port) are missing or invalid in configuration.");
            }

            var message = new MailMessage(from!, toEmail, subject, body);
            message.IsBodyHtml = true;

            using var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}