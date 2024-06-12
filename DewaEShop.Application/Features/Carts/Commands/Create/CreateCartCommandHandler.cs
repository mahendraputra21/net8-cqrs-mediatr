using AutoMapper;
using DewaEShop.Contract;
using DewaEShop.Infrastructure.Persistence;
using DewaEShop.Infrastructure.Repositories;
using Mediator;

namespace DewaEShop.Application.Features.Carts.Commands.Create
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CreateCartCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async ValueTask<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByCartIdAsync(request.CartId, cancellationToken);

            var product = await _productRepository.GetProductbyProductIdAsync(request.ProductId, cancellationToken);

            if (cart == null)
            {
                // Create a new cart if it does not exist
                if (product != null)
                {
                    var newCartId = Guid.NewGuid();
                    var cartNew = new Domain.Cart.Cart(newCartId);
                    cartNew.AddCartItem(product, request.Quantity);
                    await _cartRepository.CreateCartAsync(cartNew, cancellationToken);
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
                    cart.AddCartItem(product, request.Quantity);
                else
                    // Handle the case where the product is null
                    throw new InvalidOperationException("Product does not exist.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Use AutoMapper to map Cart to CartDto
            var cartDto = _mapper.Map<CartDto>(cart);
            return cartDto;
        }

    }
}
