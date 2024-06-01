using cqrs_mediatr.Persistence;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext db;

        public Repository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken)
        {
            await db.Set<T>().AddAsync(entity, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            db.Entry<T>(entity).State = EntityState.Modified;
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
