using cqrs_mediatr.Features.Products.Dtos;
using MediatR;

namespace cqrs_mediatr.Features.Products.Queries.Get
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
}
