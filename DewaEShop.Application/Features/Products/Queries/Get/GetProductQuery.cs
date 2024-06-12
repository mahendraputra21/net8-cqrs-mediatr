using DewaEShop.Contract;
using Mediator;

namespace DewaEShop.Application.Features.Products.Queries.Get
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
}
