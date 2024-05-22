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
           context.Products.Remove(product);
           await context.SaveChangesAsync();
        }
    }
}
