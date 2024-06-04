using AutoMapper;
using cqrs_mediatr.Domain;
using cqrs_mediatr.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByCartIdAsync(Guid cartId, CancellationToken cancellationToken);
        Task<Cart> CreateCartAsync(Cart cart, CancellationToken cancellationToken);
    }

    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly IMapper _mapper;
        public CartRepository(AppDbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        public async Task<Cart> CreateCartAsync(Cart cart, CancellationToken cancellationToken)
        {
            await InsertAsync(cart, cancellationToken);
            return cart;
        }

        public async Task<Cart?> GetCartByCartIdAsync(Guid cartId, CancellationToken cancellationToken)
        {
            return await db.Carts.Include(c => c.Items)
                                     .ThenInclude(i => i.Product)
                                     .FirstOrDefaultAsync(c=> c.Id == cartId, cancellationToken);
        }
    }
}
