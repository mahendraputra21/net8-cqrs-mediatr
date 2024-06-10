using cqrs_mediatr.Behaviors;
using cqrs_mediatr.Exceptions;
using cqrs_mediatr.Persistence;
using cqrs_mediatr.Repositories;
using FluentValidation;
using System.Diagnostics;
using System.Reflection;

namespace cqrs_mediatr.Configuration
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Registering DB Context
            services.AddDbContext<AppDbContext>();

            // Registering AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Registering Pipeline Behaviours
            services.AddScoped(typeof(RequestResponseLoggingBehavior<,>));
            services.AddScoped(typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(DBContextTransactionPipelineBehavior<,>));

            // Registering Fluent Validator
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registering Global Exception Handler
            services.AddExceptionHandler<GlobalExceptionHandler>();

            // Registering Problem Details
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (context) =>
                {
                    if (!context.ProblemDetails.Extensions.ContainsKey("traceId"))
                    {
                        string? traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
                        context.ProblemDetails.Extensions.Add(new KeyValuePair<string, object?>("traceId", traceId));
                    }
                };
            });

            // Registering Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }
    }
}
