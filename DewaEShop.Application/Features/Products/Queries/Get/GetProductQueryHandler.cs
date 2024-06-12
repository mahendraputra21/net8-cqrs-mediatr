using DewaEShop.Contract;
using DewaEShop.Infrastructure.Repositories;
using Mediator;

namespace DewaEShop.Application.Features.Products.Queries.Get
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async ValueTask<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.SelectProductByIdAsync(request.Id, cancellationToken);
            if (product == null) return null;

            return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.IsDeleted);
        }
    }
}
