using cqrs_mediatr.Behaviors;
using cqrs_mediatr.Model;
using Mediator;

namespace cqrs_mediatr.Features.Carts.Commands.Create
{
    public class CreateCartCommand : IRequest<CartDto>
    {
        private readonly IGuidGenerator _guidGenerator;
        public Guid CartId { get; set; } 
        public Guid ProductId { get; set; } 
        public int Quantity { get; set; }

        public CreateCartCommand(Guid cartId, Guid productId, int quantity, IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;

            CartId = _guidGenerator.GenerateGuid();
            ProductId = _guidGenerator.GenerateGuid();
            Quantity = quantity;
            
        }
    }

}
