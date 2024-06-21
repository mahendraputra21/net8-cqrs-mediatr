using Asp.Versioning;
using DewaEShop.Domain.User;
using DewaEShop.Exceptions;
using DewaEShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;

namespace DewaEShop.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddGlobalExceptionHandler();
            services.AddCustomProblemDetails();
            services.AddCustomAuthenticationAndAuthorization();
            services.AddIdentityConfiguration();
            services.AddSwaggerConfiguration();
            services.AddAPIVersioning();
            return services;
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
                //options.SignIn.RequireConfirmedEmail = true; // need confirmation when login 
            })
                    .AddEntityFrameworkStores<AppDbContext>();
                    //.AddApiEndpoints();
        }

        private static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DewaEShop API", Version = "v1.0" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "DewaEShop API", Version = "v2.0" });
                c.SwaggerDoc("v3", new OpenApiInfo { Title = "DewaEShop API", Version = "v3.0" });
                c.SwaggerDoc("v4", new OpenApiInfo { Title = "DewaEShop API", Version = "v4.0" });
                c.SwaggerDoc("v5", new OpenApiInfo { Title = "DewaEShop API", Version = "v5.0" });

                // Define the Bearer token security scheme
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                };
                c.AddSecurityDefinition("Bearer", securityScheme);

                // Add JWT bearer token authentication requirements
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer" 
                            }
                        },
                        new string[] { "default" }
                    }
                 });
            });
        }

        private static void AddAPIVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
