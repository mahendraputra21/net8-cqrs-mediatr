using cqrs_mediatr.Domain;
using cqrs_mediatr.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> SelectProductByIdAsync(Guid ProductId, CancellationToken cancellationToken);
        Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task<Guid> CreateProductAsync(Product product, CancellationToken cancellationToken);
        Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken);
        Task<bool> IsUniqueProductNameAsync(string productName, CancellationToken cancellationToken);
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<Guid> CreateProductAsync(Product product, CancellationToken cancellationToken)
        {
           await InsertAsync(product, cancellationToken);
           return product.Id;
        }

        public async Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            return await db.Products.ToListAsync(cancellationToken);
        }

        public async Task<bool> IsUniqueProductNameAsync(string productName, CancellationToken cancellationToken)
        {
            var isNameExists = await db.Products.AnyAsync(x => x.Name == productName, cancellationToken);
            return isNameExists;
        }

        public async Task<Product?> SelectProductByIdAsync(Guid ProductId, CancellationToken cancellationToken)
        {
            var product = await db.Products.FindAsync(ProductId, cancellationToken);
            if (product == null) return null;
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken)
        {
            await UpdateAsync(product, cancellationToken);
            return product;
        }
    }
}
