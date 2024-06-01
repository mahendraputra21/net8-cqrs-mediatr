using cqrs_mediatr.Behaviors;

namespace cqrs_mediatr.Repositories.Configuration
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
