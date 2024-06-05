using cqrs_mediatr.Behaviors;
using cqrs_mediatr.Model;
using Mediator;

namespace cqrs_mediatr.Features.Carts.Commands.Create
{
    public record CreateCartCommand(Guid CartId, Guid ProductId, int Quantity) : IRequest<CartDto>;
}
