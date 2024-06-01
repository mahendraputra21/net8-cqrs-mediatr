using AutoMapper;
using cqrs_mediatr.Model;
using cqrs_mediatr.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Features.Carts.Commands.Create
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateCartCommandHandler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _dbContext.Carts.Include(c => c.Items)
                                             .ThenInclude(i => i.Product) // Ensure product is included for price calculation
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
                    // Handle the case where the product is null
                    throw new InvalidOperationException("Product does not exist.");
            }
            else
            {
                // Add the item to the existing cart
                if (product != null)
                    cart.AddCartItem(product, request.Quatity);
                else
                    // Handle the case where the product is null
                    throw new InvalidOperationException("Product does not exist.");
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Use AutoMapper to map Cart to CartDto
            var cartDto = _mapper.Map<CartDto>(cart);

            return cartDto;
        }

    }
}
