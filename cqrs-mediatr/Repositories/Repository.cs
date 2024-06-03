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
            return entity;
        }

        public void Delete(T entity, CancellationToken cancellationToken)
        {
            db.Set<T>().Remove(entity);
        }

        public void UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            db.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
