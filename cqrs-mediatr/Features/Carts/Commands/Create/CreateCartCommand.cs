using cqrs_mediatr.Model;
using MediatR;

namespace cqrs_mediatr.Features.Carts.Commands.Create
{
    public record CreateCartCommand(Guid CartId, Guid ProductId, int Quatity) : IRequest<CartDto>;
}
