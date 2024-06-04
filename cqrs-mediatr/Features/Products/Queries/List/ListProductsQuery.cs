using cqrs_mediatr.Models;
using Mediator;

namespace cqrs_mediatr.Features.Products.Queries.List
{
    public record ListProductsQuery : IRequest<List<ProductDto>>;
}
