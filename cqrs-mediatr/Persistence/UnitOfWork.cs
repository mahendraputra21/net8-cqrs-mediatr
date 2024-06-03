using Microsoft.EntityFrameworkCore.Storage;

namespace cqrs_mediatr.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private IDbContextTransaction _dbContextTransaction;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            _dbContextTransaction =_appDbContext.Database.BeginTransaction();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellation)
        {
            try
            {
                await SaveChangesAsync(cancellation);
                _dbContextTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void RollbackTransaction()
        {
            _dbContextTransaction?.Rollback();
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
            _appDbContext?.Dispose();
        }
        
    }
}
