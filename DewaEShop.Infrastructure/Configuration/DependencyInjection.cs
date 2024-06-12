using DewaEShop.Infrastructure.Behaviors;
using DewaEShop.Infrastructure.Persistence;
using DewaEShop.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DewaEShop.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddMediator();
            services.AddPipelineBehaviors();
            services.AddFluentValidation();
            services.AddRepositories();
            return services;
        }

        private static void AddPipelineBehaviors(this IServiceCollection services)
        {
            services.AddScoped(typeof(RequestResponseLoggingBehavior<,>));
            services.AddScoped(typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(DBContextTransactionPipelineBehavior<,>));
        }

        private static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void AddMediator(this IServiceCollection services) 
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });
        }
    }
}
