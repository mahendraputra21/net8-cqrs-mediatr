using DewaEShop.Infrastructure.Persistence;
using DewaEShop.Infrastructure.Repositories;
using Mediator;

namespace DewaEShop.Application.Features.Products.Commands.Delete
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

        public async ValueTask<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.SelectProductByIdAsync(request.Id, cancellationToken);
            if (product == null) throw new ArgumentException("Product is not found");

            product.InactiveProduct(product.IsDeleted);

            _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
