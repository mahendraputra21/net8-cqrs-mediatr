using DewaEShop.Application.Behaviors;
using DewaEShop.Application.Features.Products.Commands.Create;
using DewaEShop.Application.Features.Products.Commands.Update;
using DewaEShop.Application.Mapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DewaEShop.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatorInjection();
            services.AddIValidator();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddPipelineBehaviors();
            services.AddFluentValidation();
            return services;
        }

        private static void AddIValidator(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
            services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
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

        private static void AddMediatorInjection(this IServiceCollection services)
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });
        }
    }
}
