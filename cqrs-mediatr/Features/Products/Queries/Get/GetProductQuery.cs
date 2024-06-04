using cqrs_mediatr.Models;
using Mediator;

namespace cqrs_mediatr.Features.Products.Queries.Get
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
}
