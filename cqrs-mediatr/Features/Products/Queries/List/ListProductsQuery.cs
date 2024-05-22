using cqrs_mediatr.Features.Products.Dtos;
using MediatR;

namespace cqrs_mediatr.Features.Products.Queries.List
{
    public record ListProductsQuery : IRequest<List<ProductDto>>;
}
