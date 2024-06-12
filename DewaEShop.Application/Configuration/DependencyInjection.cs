using DewaEShop.Application.Features.Products.Commands.Create;
using DewaEShop.Application.Features.Products.Commands.Update;
using DewaEShop.Application.Mapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DewaEShop.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddIValidator();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        private static void AddIValidator(this IServiceCollection services) 
        {
            services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
            services.AddScoped<IValidator<UpdateProductCommand>,  UpdateProductCommandValidator>();
        }
    }
}
