using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace DewaEShop.SendGrid.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSendGridDependencyInjection(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Use SendGrid Key
            services.AddSendGrid(options =>
            {
                options.ApiKey = builder.Configuration["SendGridConfig:APIKey"];
            });

            services.AddTransient<ISendGridEmailSender, SendGridEmailSender>();
            return services;
        }
    }
}
