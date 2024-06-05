using cqrs_mediatr.Behaviors;
using cqrs_mediatr.Model;
using Mediator;

namespace cqrs_mediatr.Features.Carts.Commands.Create
{
    public class CreateCartCommand : IRequest<CartDto>
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IInt32Generator _int32Generator;

        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; } 
        public int Quantity { get; private set; }

        public CreateCartCommand(Guid CartId, Guid ProductId, int Quantity, IGuidGenerator typeDataGenerator, IInt32Generator int32Generator)
        {
            _guidGenerator = typeDataGenerator;
            _int32Generator = int32Generator;
            CartId = _guidGenerator.GenerateGuid();
            ProductId = _guidGenerator.GenerateGuid();
            Quantity = _int32Generator.GenerateInt32();
            
        }
    }
}
