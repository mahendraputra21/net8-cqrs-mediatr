using cqrs_mediatr.Features.Carts.Commands.Create;
using cqrs_mediatr.Persistence;

namespace cqrs_mediatr.Repositories.Configuration
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}
