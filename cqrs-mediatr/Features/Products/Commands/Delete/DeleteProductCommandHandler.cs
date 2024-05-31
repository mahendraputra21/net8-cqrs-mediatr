using cqrs_mediatr.Persistence;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler(AppDbContext context) : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           var product = await context.Products.FindAsync(request.Id);
           if (product == null) return;
           
           product.InactiveProduct(product.IsDeleted);
           
           context.Products.Update(product);
           await context.SaveChangesAsync(cancellationToken);
        }
    }
}
