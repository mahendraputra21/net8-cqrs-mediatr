using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;

namespace SendGrid.Lib
{
    public interface ISendGridEmailSender
    {
        Task SendEmailWithTemplateAsync(string toEmail, string subject, string templateId);
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

        public async Task SendEmailWithTemplateAsync(string toEmail, string subject, string templateId)
        {

            var from = new EmailAddress(_configuration["SendGridConfig:From"], _configuration["SendGridConfig:Name"]);
            var to = new EmailAddress(toEmail);
            var dynamicEmailData = new
            {
                SUBJECT = subject,
                FULLNAME = "John Pantau",
                LOGO = "https://dev-portal.invcar.com/assets/InvCar-Dark-Horizontal.png"
            };
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicEmailData);
            var response = await _sendGridClient.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Email has been sent successfully");
            }
        }
    }
}
