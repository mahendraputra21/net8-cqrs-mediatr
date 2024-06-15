using DewaEShop.SendGrid.Configuration;
using Microsoft.Extensions.Configuration;
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

        public SendGridEmailSender(ISendGridClient sendGridClient, IOptionsMonitor<SendGridConfig> sendGridConfig)
        {
            _sendGridClient = sendGridClient;
            _sendGridConfig = sendGridConfig.CurrentValue;
        }

        public async Task SendEmailWithTemplateAsync(string toEmail, string templateId, object emailData)
        {

            var from = new EmailAddress(_sendGridConfig.FromName, _sendGridConfig.FromEmailAddress);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, emailData);
            var response = await _sendGridClient.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Email has been sent successfully");
            }
        }
    }
}
