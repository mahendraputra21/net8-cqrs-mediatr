using DewaEShop.SendGrid.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace DewaEShop.SendGrid
{
    public interface ISendGridEmailSender
    {
        Task SendEmailWithTemplateAsync(string toEmail, string templateId, object emailData);
    }

    public class SendGridEmailSender : ISendGridEmailSender
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly SendGridConfig _sendGridConfig;
        private readonly ILogger<SendGridEmailSender> _logger;

        public SendGridEmailSender(
            ISendGridClient sendGridClient,
            IOptionsMonitor<SendGridConfig> sendGridConfig,
            ILogger<SendGridEmailSender> logger)
        {
            _sendGridClient = sendGridClient;
            _sendGridConfig = sendGridConfig.CurrentValue;
            _logger = logger;
        }

        public async Task SendEmailWithTemplateAsync(string toEmail, string templateId, object emailData)
        {

            var from = new EmailAddress(_sendGridConfig.FromName, _sendGridConfig.FromEmailAddress);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, emailData);
            var response = await _sendGridClient.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email has been sent successfully to {Email}", toEmail);
            }
            else
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("Error sending email to {Email}: {StatusCode} {ResponseBody}", toEmail, response.StatusCode, responseBody);
                _logger.LogError("From Email Address: {FromEmailAddress}", _sendGridConfig.FromEmailAddress);
            }
        }
    }
}
