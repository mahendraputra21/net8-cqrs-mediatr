namespace cqrs_mediatr.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> InsertAsync(T entity, CancellationToken cancellationToken);
        void UpdateAsync(T entity, CancellationToken cancellationToken);
        void Delete(T entity, CancellationToken cancellationToken);
    }
}
