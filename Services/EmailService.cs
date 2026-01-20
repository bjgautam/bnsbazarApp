using System.Net;
using System.Net.Mail;

namespace BnsBazarApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient(_config.GetValue<string>("EmailSettings:Host"))
            {
                Port = _config.GetValue<int>("EmailSettings:Port"),
                EnableSsl = _config.GetValue<bool>("EmailSettings:EnableSSL"),
                Credentials = new NetworkCredential(
                    _config.GetValue<string>("EmailSettings:Username"),
                    _config.GetValue<string>("EmailSettings:Password")
                )
            };

            var fromEmail = _config.GetValue<string>("EmailSettings:Username");

            if (string.IsNullOrWhiteSpace(fromEmail))
                throw new Exception("Email username is missing in configuration.");

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await smtp.SendMailAsync(mail);
        }
    }
}