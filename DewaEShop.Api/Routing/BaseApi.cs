using Asp.Versioning;
using Asp.Versioning.Builder;

namespace DewaEShop.Routing
{
    public class BaseApi
    {
        public readonly ApiVersionSet ApiVersionSet;

        public BaseApi(IEndpointRouteBuilder endpoints)
        {
            ApiVersionSet = CreateApiVersionSet(endpoints);
        }

        private ApiVersionSet CreateApiVersionSet(IEndpointRouteBuilder endpoints) =>
        endpoints.NewApiVersionSet()
                 .HasApiVersion(new ApiVersion(1, 0))
                 .HasApiVersion(new ApiVersion(2, 0))
                 .HasApiVersion(new ApiVersion(3, 0))
                 .HasApiVersion(new ApiVersion(4, 0))
                 .HasApiVersion(new ApiVersion(5, 0))
                 .ReportApiVersions()
                 .Build();

        public RouteGroupBuilder CreateRouteGroup(IEndpointRouteBuilder endpoints, string routePrefix)
        {
            return endpoints
                .MapGroup($"api/v{{version:apiVersion}}/{routePrefix}")
                .WithApiVersionSet(ApiVersionSet);
        }
    }
}
