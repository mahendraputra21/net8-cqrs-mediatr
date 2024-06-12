using DewaEShop.Contract;
using Mediator;

namespace DewaEShop.Application.Features.Products.Queries.List
{
    public record ListProductsQuery : IRequest<List<ProductDto>>;
}
