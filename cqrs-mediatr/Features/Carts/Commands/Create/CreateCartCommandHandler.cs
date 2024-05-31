using cqrs_mediatr.Model;
using cqrs_mediatr.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Features.Carts.Commands.Create
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly AppDbContext _dbContext;

        public CreateCartCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _dbContext.Carts.Include(c => c.Items)
                                             .FirstOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (cart == null)
            {
                // Create a new cart if it does not exist
                if (product != null)
                {
                    var newCartId = Guid.NewGuid();
                    var cartNew = new Domain.Cart(newCartId);
                    cartNew.AddCartItem(product, request.Quatity);
                    await _dbContext.Carts.AddAsync(cartNew, cancellationToken);
                    cart = cartNew; // Assign the new cart to the cart variable
                }
                else
                {
                    // Handle the case where the product is null
                    throw new InvalidOperationException("Product does not exist.");
                }
            }
            else
            {
                // Add the item to the existing cart
                if (product != null)
                {
                    cart.AddCartItem(product, request.Quatity);
                }
                else
                {
                    // Handle the case where the product is null
                    throw new InvalidOperationException("Product does not exist.");
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Manual mapping
            var cartItemsDto = cart.Items.Select(item => new CartItemDto(
                item.Product.Id,
                item.Product.Name,
                item.Product.Description,
                item.Quantity
            )).ToList();

            var cartDto = new CartDto(
                cart.Id,
                cartItemsDto,
                cart.TotalPrice
            );

            return cartDto;
        }

    }
}
