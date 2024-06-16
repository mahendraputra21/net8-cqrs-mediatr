using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace DewaEShop.SendGrid.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSendGridDependencyInjection(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.Configure<SendGridConfig>(builder.Configuration.GetSection("SendGridConfig"));

            // Use SendGrid Key
            services.AddSendGrid(options =>
            {
                var sendGridConfig = builder.Configuration.GetSection("SendGridConfig").Get<SendGridConfig>();
                options.ApiKey = sendGridConfig?.APIKey;
            });

            services.AddTransient<ISendGridEmailSender, SendGridEmailSender>();
            services.AddTransient<IEmailSender, RegisterEmailSender>();
            return services;
        }
    }
}
