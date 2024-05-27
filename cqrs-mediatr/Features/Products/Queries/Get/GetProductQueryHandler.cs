using cqrs_mediatr.Models;
using cqrs_mediatr.Persistence;
using MediatR;

namespace cqrs_mediatr.Features.Products.Queries.Get
{
    public class GetProductQueryHandler(AppDbContext context) : IRequestHandler<GetProductQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return null;
            }
            return new ProductDto(product.Id, product.Name, product.Description, product.Price);
        }
    }
}
