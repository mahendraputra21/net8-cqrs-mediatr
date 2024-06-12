using DewaEShop.Contract;
using Mediator;

namespace DewaEShop.Application.Features.Carts.Commands.Create
{
    public record CreateCartCommand(Guid CartId, Guid ProductId, int Quantity) : IRequest<CartDto>;
}
