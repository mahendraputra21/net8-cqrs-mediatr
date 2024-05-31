using cqrs_mediatr.Persistence;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler(AppDbContext context) : IRequestHandler<UpdateProductCommand, Guid>
    {
        public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(request.Id, cancellationToken);
            
            if (product == null) 
                return Guid.Empty;

            product.UpdateProduct(request.Name, request.Description, request.Price);
            context.Products.Update(product);
            await context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
