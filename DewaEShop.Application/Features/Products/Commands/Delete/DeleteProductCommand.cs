using Mediator;

namespace DewaEShop.Application.Features.Products.Commands.Delete
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}
