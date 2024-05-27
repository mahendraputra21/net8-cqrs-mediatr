using cqrs_mediatr.Models;
using MediatR;

namespace cqrs_mediatr.Features.Products.Queries.List
{
    public record ListProductsQuery : IRequest<List<ProductDto>>;
}
