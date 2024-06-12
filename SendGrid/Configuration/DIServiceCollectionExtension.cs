using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace SendGrid.Lib.Configuration
{
    public static class DIServiceCollectionExtension
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
