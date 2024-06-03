using System.Data;

namespace cqrs_mediatr.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        void RollbackTransaction();
    }
}
