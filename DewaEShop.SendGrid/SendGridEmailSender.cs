using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public SendGridEmailSender(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailWithTemplateAsync(string toEmail, string templateId, object emailData)
        {

            var from = new EmailAddress(_configuration["SendGridConfig:From"], _configuration["SendGridConfig:Name"]);
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
