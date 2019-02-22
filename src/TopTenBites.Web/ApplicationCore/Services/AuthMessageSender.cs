using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AppSettingsOptions Options { get; }

        public AuthMessageSender(IOptions<AppSettingsOptions> options)
        {
            Options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return ExecuteEmail(Options.SendGridApiKey, subject, message, email);
        }

        private Task ExecuteEmail(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.EmailAddress, Options.ApplicationName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }


    }
}
