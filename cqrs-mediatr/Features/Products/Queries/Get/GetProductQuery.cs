using cqrs_mediatr.Models;
using MediatR;

namespace cqrs_mediatr.Features.Products.Queries.Get
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
}
