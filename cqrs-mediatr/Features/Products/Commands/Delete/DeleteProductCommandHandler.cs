using cqrs_mediatr.Repositories;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           var product = await _productRepository.SelectProductByIdAsync(request.Id, cancellationToken);
           if (product == null) return;
           
           product.InactiveProduct(product.IsDeleted);
           
           await _productRepository.UpdateAsync(product, cancellationToken);
        }
    }
}
