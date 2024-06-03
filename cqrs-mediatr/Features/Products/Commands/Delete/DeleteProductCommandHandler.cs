using cqrs_mediatr.Persistence;
using cqrs_mediatr.Repositories;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           var product = await _productRepository.SelectProductByIdAsync(request.Id, cancellationToken);
           if (product == null) return;
           
           product.InactiveProduct(product.IsDeleted);
           
           _productRepository.UpdateAsync(product, cancellationToken);
           await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
