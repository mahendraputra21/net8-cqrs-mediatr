using cqrs_mediatr.Persistence;
using MediatR;

namespace cqrs_mediatr.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler(AppDbContext context) : IRequestHandler<UpdateProductCommand, Guid>
    {
        public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(request.Id);
            if (product == null) return Guid.Empty;
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return product.Id;
        }
    }
}
