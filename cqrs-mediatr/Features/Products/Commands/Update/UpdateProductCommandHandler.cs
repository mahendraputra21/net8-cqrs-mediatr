using cqrs_mediatr.Repositories;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.SelectProductByIdAsync(request.Id, cancellationToken);
            
            if (product == null) 
                return Guid.Empty;

            product.UpdateProduct(request.Name, request.Description, request.Price);
            await _productRepository.UpdateProductAsync(product, cancellationToken);
            return product.Id;
        }
    }
}
