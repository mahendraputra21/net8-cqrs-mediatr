using cqrs_mediatr.Domain;
using cqrs_mediatr.Repositories;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product(command.Name, command.Description, command.Price);
            var productId = await _productRepository.CreateProductAsync(product, cancellationToken);
            return productId;
        }
    }
}
