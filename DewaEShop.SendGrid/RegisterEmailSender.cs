using DewaEShop.SendGrid.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DewaEShop.SendGrid
{
    public class RegisterEmailSender : IEmailSender
    {
        private readonly SendGridConfig _sendGridConfig;
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<RegisterEmailSender> _logger;

        public RegisterEmailSender(
            IOptionsMonitor<SendGridConfig> sendGridConfig,
            ISendGridClient sendGridClient,
            ILogger<RegisterEmailSender> logger)
        {
            _sendGridConfig = sendGridConfig.CurrentValue;
            _sendGridClient = sendGridClient;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmailAddress = _sendGridConfig.FromEmailAddress;

            var message = new SendGridMessage
            {
                From = new EmailAddress(_sendGridConfig.FromEmailAddress,
                                        _sendGridConfig.FromName),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };

            message.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            var response =await _sendGridClient.SendEmailAsync(message);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email has been sent successfully to {Email}", email);
            }
            else
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("Error sending email to {Email}: {StatusCode} {ResponseBody}", email, response.StatusCode, responseBody);
                _logger.LogError("From Email Address: {FromEmailAddress}", fromEmailAddress);
            }
        }
    }
}
