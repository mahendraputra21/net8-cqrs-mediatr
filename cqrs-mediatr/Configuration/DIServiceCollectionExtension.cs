using cqrs_mediatr.Behaviors;
using cqrs_mediatr.Domain;
using cqrs_mediatr.Exceptions;
using cqrs_mediatr.Persistence;
using cqrs_mediatr.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;

namespace cqrs_mediatr.Configuration
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddPipelineBehaviors();
            services.AddFluentValidation();
            services.AddGlobalExceptionHandler();
            services.AddCustomProblemDetails();
            services.AddCustomAuthenticationAndAuthorization();
            services.AddIdentityConfiguration();
            services.AddRepositories();
            services.AddSwaggerConfiguration();

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

        private static void AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        private static void AddCustomProblemDetails(this IServiceCollection services)
        {
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
        }

        private static void AddCustomAuthenticationAndAuthorization(this IServiceCollection services)
        {
            // simple authentication 
            services.AddAuthentication()
                    .AddBearerToken(IdentityConstants.BearerScheme);

            services.AddAuthorizationBuilder();
        }

        private static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true; // need confirmation when login 
            })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddApiEndpoints();
        }

        private static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CQRS Mediator API", Version = "v1" });

                // Define the Bearer token security scheme
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                c.AddSecurityDefinition("Bearer", securityScheme);

                // Add JWT bearer token authentication requirements
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                 });
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
