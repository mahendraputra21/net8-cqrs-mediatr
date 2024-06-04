using cqrs_mediatr.Persistence;

namespace cqrs_mediatr.Repositories.Configuration
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            return services;
        }
    }
}
